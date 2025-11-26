using LiteDB;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using PDFIndexer.BackgroundTask;
using PDFIndexer.Journal;
using PDFIndexer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace PDFIndexer.SearchEngine
{
    internal class Indexer
    {
        private LuceneProvider Provider;

        public Indexer(LuceneProvider provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// 주어진 PDF 파일들을 인덱싱합니다.
        /// </summary>
        /// <param name="pdfs">인덱싱할 pdf들의 경로입니다</param>
        public void IndexPdfs(string[] pdfs)
        {
            Logger.Write(JournalLevel.Info, $"IndexPdfs 요청 - 요청된 PDF 수:{pdfs.Length}");

            var writer = Provider.GetIndexWriter();

            var dbCollection = DBContext.DB.GetCollection<IndexedDocument>("indexed");

            foreach (var path in pdfs)
            {
                // 프로그램 종료 체크
                if (Program.Disposing) return;

                var filename = (path.Split('\\').LastOrDefault() ?? path).Replace(".pdf", "");

                string hash = IOUtil.GetHashFromFile(path);
                long lastModified = IOUtil.GetLastModifiedFromFile(path);

                using (PdfDocument pdf = PdfDocument.Open(path))
                {
                    // 페이지 별로 인덱스
                    var pages = pdf.GetPages();
                    foreach (var page in pages)
                    {
                        // 프로그램 종료 체크
                        if (Program.Disposing) return;

                        // 기본 메타데이터
                        Document doc = new Document
                        {
                            new TextField("title", filename, Field.Store.YES),
                            new StringField("path", path, Field.Store.YES),
                            new Int32Field("page", page.Number, Field.Store.YES),
                            new StringField("md5", hash, Field.Store.YES),
                            new StringField("lastModified", lastModified.ToString(), Field.Store.YES),
                        };

                        // 페이지 내용 (텍스트만)
                        string content = ContentOrderTextExtractor.GetText(page);
                        doc.Add(new TextField("content", content, Field.Store.YES));

                        // DB 업데이트
                        var dbItem = new IndexedDocument
                        {
                            _id = $"{path}//{page.Number}",
                            Path = path,
                            Page = page.Number,
                            MD5 = hash,
                            LastModified = lastModified,
                            OCRDone = false,
                        };
                        dbCollection.Upsert(dbItem);

                        // 인덱스 추가
                        writer.AddDocument(doc);

                        // 이미지 OCR task enqueue
                        TaskManager.Enqueue(new OCRTask(path, page.Number));
                    }

                    // Logger.Write($"IndexPdfs - Index done: {path} with {pages.Count()} pages");
                }
            }

            if (Program.Disposing) return;

            Logger.Write(JournalLevel.Info, "IndexPdfs 정리 중..");
            writer.Commit();
            writer.Dispose();

            Logger.Write(JournalLevel.Info, "IndexPdfs 완료.");
        }

        public void CleanupIndexes()
        {
            // 인덱스를 업데이트하기 위한 reader/writer 가져오기
            var writer = Provider.GetIndexWriter();
            var reader = Provider.GetIndexReader();

            // 업데이트가 필요한 문서
            List<string> toUpdate = new List<string>();

            // 인덱스를 구성하는 모든 리프 순회
            foreach (var leafContext in reader.Leaves)
            {
                var leadReader = leafContext.Reader;

                // 삭제되지 않은 문서 정보 가져오기
                var liveDocs = MultiFields.GetLiveDocs(leadReader);

                // 모든 문서 순회
                for (int id = 0; id < leadReader.MaxDoc; id++)
                {
                    // 삭제된 문서 스킵
                    if (liveDocs != null && !liveDocs.Get(id)) continue;

                    Document document = leadReader.Document(id);

                    string path = document.Get("path");
                    if (!File.Exists(path))
                    {
                        writer.DeleteDocuments(new Term("path", path));
                        continue;
                    }

                    // 해시 구하는 건 CPU 시간을 많이 사용하니, cron task에서는 미사용.
                    // hash로 비교는 toIndexingFiles < indexedFiles 일 때만 사용.
                    //
                    // 해당 path에는 존재하는데 hash가 다르면
                    // 파일이 업데이트/변경된거 --> 새로 인덱싱 해야함
                    // string indexedHash = document.Get("hash");
                    // string realHash = GetHashFromFile(path);

                    // 인덱스의 last modified와 실제 last modified의 시간이 다르면
                    // 파일이 변경되었으므로, 새로 인덱싱 해야 함
                    long lastModifiedFromIndex = Int64.Parse(document.Get("lastModified"));
                    long lastModifiedFromFile = IOUtil.GetLastModifiedFromFile(path);
                    if (lastModifiedFromIndex != lastModifiedFromFile)
                    {
                        toUpdate.Add(path);
                    }
                }
            }

            // 삭제 완료 --> 인덱스 저장
            writer.Commit();
            writer.Dispose();

            Provider.Initialize();

            // 업데이트가 필요한 문서들 업데이트
            //indexer.IndexPdfs(toUpdate.ToArray());
        }
    }
}
