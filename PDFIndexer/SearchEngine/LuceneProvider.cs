using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cjk;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using PDFIndexer.Journal;
using System;
using System.IO;
using LuceneDirectory = Lucene.Net.Store.Directory;

namespace PDFIndexer.SearchEngine
{
    public class LuceneProvider : IDisposable
    {
        private string BasePath;

        private const LuceneVersion luceneVersion = LuceneVersion.LUCENE_48;

        private static LuceneDirectory IndexDirectory;
        private static Analyzer Analyzer;
        private static IndexWriter Writer;
        [Obsolete("Deprecated. Use `GetIndexSearcher` instead.")]
        private static IndexReader Reader;
        private static SearcherManager SearcherManager;

        private static bool _IsDisposed;

        private static bool _Ready = false;
        public static bool Ready { get { return _Ready; } }

        private static bool _NotIndexedYet = true;
        public static bool NotIndexedYet { get { return _NotIndexedYet; } }

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
            if (!_IsDisposed)
            {
                Logger.Write(JournalLevel.Info, "Indexer Ready");
                OnReady?.Invoke();
            }
        }

        public void Initialize()
        {
            if (_IsDisposed) return;
            if (Program.Disposing) return;

            Logger.Write(JournalLevel.Info, "LuceneProvider 초기화");
            SetReadyState(false);

            _NotIndexedYet = !File.Exists(Path.Combine(BasePath, ".indexed"));

            IndexDirectory = FSDirectory.Open(Path.Combine(BasePath, "index"));
            Analyzer = new CJKAnalyzer(luceneVersion);

            GetIndexWriter();
            SearcherManager = new SearcherManager(Writer, true, null);

            //try
            //{
            //    Searcher = InitializeIndexSearcher();
            //    // SetReadyState(true);

            //    Logger.Write("기존 인덱스 찾음");
            //}
            //catch (DirectoryNotFoundException)
            //{
            //    // 인덱스 없음
            //    Searcher = null;

            //    Logger.Write(JournalLevel.Warning, "디스크에 저장된 인덱스 없음");
            //}

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
            if (Writer == null)
            {
                IndexWriterConfig indexConfig = new IndexWriterConfig(luceneVersion, Analyzer);
                indexConfig.OpenMode = OpenMode.CREATE_OR_APPEND;
                Writer = new IndexWriter(IndexDirectory, indexConfig);
            }

            return Writer;
        }

        public void ReplaceIndexWriter()
        {
            try
            {
                Writer.Dispose();
            }
            finally
            {
                if (IndexWriter.IsLocked(IndexDirectory))
                {
                    IndexWriter.Unlock(IndexDirectory);
                }
            }

            IndexWriterConfig indexConfig = new IndexWriterConfig(luceneVersion, Analyzer);
            indexConfig.OpenMode = OpenMode.CREATE;
            Writer = new IndexWriter(IndexDirectory, indexConfig);
        }

        [Obsolete("Deprecated. Use `GetIndexSearcher` instead.")]
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
            return SearcherManager.Acquire();
        }

        public void ReleaseSearcher(IndexSearcher searcher)
        {
            SearcherManager.Release(searcher);
        }

        public void MarkAsDirty()
        {
            SearcherManager.MaybeRefresh();
        }

        public void MarkDoneFirstIndex()
        {
            var path = Path.Combine(BasePath, ".indexed");
            if (File.Exists(path)) return;

            File.Create(path);
            _NotIndexedYet = true;
        }
    }
}
