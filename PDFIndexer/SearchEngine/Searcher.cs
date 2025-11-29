using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.QueryParsers.Simple;
using Lucene.Net.Search;
using Lucene.Net.Util;
using PDFIndexer.Models;
using PDFIndexer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFIndexer.SearchEngine
{
    internal class Searcher
    {
        private LuceneProvider Provider;

        public Searcher(LuceneProvider provider)
        {
            Provider = provider;
        }

        /// <summary>주어진 쿼리로 검색</summary>
        /// <param name="text">검색 쿼리. <c>IsNullOrWhitespace</c>가 아닌 <c>string</c>이어야 함</param>
        /// <param name="max">최대 반환 결과. 검색 자체는 최대한 하지만, 일치도가 높은 상위 최대 <c>max</c>개를 반환합니다.</param>
        /// <returns>Nullable&lt;TopDocs&gt;를 반환</returns>
        public TopDocs SearchQuery(string text, int max = 10)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            var analyzer = Provider.GetAnalyzer();
            var providerSearcher = Provider.GetIndexSearcher();

            SimpleQueryParser parser = new SimpleQueryParser(analyzer, "content");
            Query query = parser.Parse(text);
            if (query == null) return null;
            TopDocs topDocs = providerSearcher.Search(query, max);

            Provider.ReleaseSearcher(providerSearcher);

            return topDocs;
        }

        public DocumentGroup[] GroupDocuments(ScoreDoc[] scoreDocs)
        {
            var searcher = SearchEngineContext.Provider.GetIndexSearcher();

            // 경로를 기준으로 그룹
            Dictionary<string, DocumentGroup> result = new Dictionary<string, DocumentGroup>();

            foreach (var scoreDoc in scoreDocs)
            {
                Document doc = searcher.Doc(scoreDoc.Doc);
                string path = doc.Get("path");
                int page = int.Parse(doc.Get("page"));

                if (result.ContainsKey(path))
                {
                    if (result.TryGetValue(path, out DocumentGroup group))
                    {
                        result.Remove(path);
                        group.Add(page, scoreDoc);
                        result.Add(path, group);
                    }
                }
                else
                {
                    string title = doc.Get("title");

                    var group = new DocumentGroup(title, path);
                    group.Add(page, scoreDoc);

                    result.Add(path, group);
                }
            }

            var resultArray = result.Values.ToArray();
            Array.Sort(resultArray, (a, b) => b.TotalScore.CompareTo(a.TotalScore));

            SearchEngineContext.Provider.ReleaseSearcher(searcher);

            return resultArray;
        }

        private bool IsMissingPdf(string pdf, bool strict = false)
        {
            string queryRaw = pdf;
            if (strict) queryRaw = IOUtil.GetHashFromFile(pdf);

            Term term;
            if (strict)
                term = new Term("md5", queryRaw);
            else
                term = new Term("path", queryRaw);

            var searcher = Provider.GetIndexSearcher();
            Query query = new TermQuery(term);
            TopDocs topDocs = searcher.Search(query, n: 1);

            // 결과 없음 == 인덱싱 안됨
            //TopDocs topDocs = indexer.;
            if (topDocs.ScoreDocs.Length == 0) return true;

            // 마지막 수정 시간이 다르면 인덱스 업데이트 --> 없다고 간주
            Document document = searcher.Doc(topDocs.ScoreDocs[0].Doc);
            long lastModifiedFromIndex = Int64.Parse(document.Get("lastModified"));
            long lastModifiedFromFile = IOUtil.GetLastModifiedFromFile(pdf);

            Provider.ReleaseSearcher(searcher);

            if (lastModifiedFromIndex != lastModifiedFromFile) return true;
            return false;
        }

        /// <summary>
        /// 인덱싱되어있지 않은 PDF를 찾습니다.
        /// </summary>
        /// <param name="indexer">인덱서</param>
        /// <param name="pdfs">검사할 PDF들의 Path를 담은 array</param>
        /// <param name="strict">(Optional) 엄격 검사 모드. 경로가 아닌 Hash로 검사합니다.</param>
        /// <returns>
        /// 인덱싱되어있지 않은 PDF의 Path를 반환합니다.
        /// </returns>
        public string[] GetMissingPdfs(string[] pdfs, bool strict = false)
        {
            var missing = new List<string>();

            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 5,
            };

            Parallel.ForEach(pdfs, parallelOptions, (pdf, _) =>
            {
                if (IsMissingPdf(pdf, strict)) missing.Add(pdf);
            });

            return missing.ToArray();
        }

        public string[] GetDuplicateFiles()
        {
            var reader = Provider.GetIndexReader();

            HashSet<string> hashes = new HashSet<string>();
            HashSet<string> duplicates = new HashSet<string>();

            // 인덱스를 구성하는 모든 리프 순회
            foreach (var leafContext in reader.Leaves)
            {
                var leafReader = leafContext.Reader;

                // 삭제되지 않은 문서 정보 가져오기
                var liveDocs = MultiFields.GetLiveDocs(leafReader);

                // 모든 문서 순회
                for (int id = 0; id < leafReader.MaxDoc; id++)
                {
                    // 삭제된 문서 스킵
                    if (liveDocs != null && !liveDocs.Get(id)) continue;

                    Document document = leafReader.Document(id);

                    string hash = document.Get("md5");
                    if (hashes.Contains(hash))
                    {
                        duplicates.Add(hash);
                    }
                    else
                    {
                        hashes.Add(hash);
                    }
                }
            }

            return duplicates.ToArray();
        }

        public SearchItem[] GetFilesFromHash(string hash)
        {
            var searcher = Provider.GetIndexSearcher();

            List<SearchItem> result = new List<SearchItem>();

            Term term = new Term("md5", hash);
            Query query = new TermQuery(term);
            TopDocs topDocs = searcher.Search(query, int.MaxValue);

            for (int i = 0; i < topDocs.ScoreDocs.Length; i++)
            {
                var doc = searcher.Doc(topDocs.ScoreDocs[i].Doc);
                result.Add(new SearchItem(doc));
            }

            Provider.ReleaseSearcher(searcher);

            return result.ToArray();
        }
    }
}
