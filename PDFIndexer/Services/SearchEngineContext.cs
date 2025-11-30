using PDFIndexer.SearchEngine;
using System.IO;

namespace PDFIndexer.Services
{
    internal class SearchEngineContext
    {
        private static readonly Properties.Settings AppSettings = Properties.Settings.Default;

        private static LuceneProvider _Provider;
        public static LuceneProvider Provider { get { return _Provider; } }

        public SearchEngineContext(string basePath)
        {
            _Provider = new LuceneProvider(Path.Combine(basePath));
        }

        public static void Dispose()
        {
            _Provider?.Dispose();
        }
    }
}
