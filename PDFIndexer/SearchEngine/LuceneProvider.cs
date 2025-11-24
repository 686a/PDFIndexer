using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cjk;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using PDFIndexer.Journal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuceneDirectory = Lucene.Net.Store.Directory;

namespace PDFIndexer.SearchEngine
{
    public class LuceneProvider : IDisposable
    {
        private string BasePath;

        private const LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;

        private static LuceneDirectory IndexDirectory;
        private static Analyzer Analyzer;
        private static IndexReader Reader;
        private static IndexSearcher Searcher;

        private bool _IsDisposed;

        private static bool _Ready = false;
        public static bool Ready { get { return _Ready; } }

        /// <summary>
        /// Indexer가 준비될때 발생합니다. 이벤트가 Main Thread에서 발생하지 않을 수도 있습니다.
        /// </summary>
        public static event ReadyEventHandler OnReady;

        public delegate void ReadyEventHandler();

        public LuceneProvider(string basePath)
        {
            BasePath = basePath;
        }

        ~LuceneProvider()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_IsDisposed) return;

            SetReadyState(false);

            if (disposing)
            {
                if (IndexDirectory != null) IndexDirectory.Dispose();
                if (Analyzer != null) Analyzer.Dispose();
                if (Reader != null) Reader.Dispose();
            }

            _IsDisposed = true;
        }

        private void SetReadyState(bool ready)
        {
            _Ready = ready;

            if (ready) DispatchReadyEvent();
        }

        private void DispatchReadyEvent()
        {
            Logger.Write(JournalLevel.Info, "Indexer Ready");
            OnReady?.Invoke();
        }

        public void Initialize()
        {
            Logger.Write(JournalLevel.Info, "LuceneProvider 초기화");
            SetReadyState(false);

            IndexDirectory = FSDirectory.Open(Path.Combine(BasePath, ".index"));
            Analyzer = new CJKAnalyzer(luceneVersion);

            try
            {
                Searcher = InitializeIndexSearcher();
                // SetReadyState(true);

                Logger.Write("기존 인덱스 찾음");
            }
            catch (DirectoryNotFoundException)
            {
                // 인덱스 없음
                Searcher = null;

                Logger.Write(JournalLevel.Warning, "디스크에 저장된 인덱스 없음");
            }

            Logger.Write(JournalLevel.Info, "LuceneProvider 초기화 --> 완료");

            SetReadyState(true);
        }

        private IndexSearcher InitializeIndexSearcher()
        {
            Reader = DirectoryReader.Open(IndexDirectory);
            return new IndexSearcher(Reader);
        }

        public IndexWriter GetIndexWriter()
        {
            IndexWriterConfig indexConfig = new IndexWriterConfig(luceneVersion, Analyzer);
            indexConfig.OpenMode = OpenMode.CREATE;
            IndexWriter writer = new IndexWriter(IndexDirectory, indexConfig);

            return writer;
        }

        public IndexReader GetIndexReader()
        {
            return Reader;
        }

        public Analyzer GetAnalyzer()
        {
            return Analyzer;
        }

        public IndexSearcher GetIndexSearcher()
        {
            return Searcher;
        }

        public Document SearchDocument(int doc)
        {
            return Searcher.Doc(doc);
        }
    }
}
