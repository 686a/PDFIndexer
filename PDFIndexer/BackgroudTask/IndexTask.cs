using PDFIndexer.BackgroundTask;
using PDFIndexer.SearchEngine;
using PDFIndexer.Services;

namespace PDFIndexer.BackgroudTask
{
    internal class IndexTask : AbstractTask
    {
        private static readonly Properties.Settings AppSettings = Properties.Settings.Default;

        public override string Name => "인덱스";
        public override string Description => $"{Path.Replace(AppSettings.BasePath, "")}";

        private string Path;

        public IndexTask( string path)
        {
            Path = path;
        }

        public override void Run()
        {
            new Indexer(SearchEngineContext.Provider)
                .IndexPdfs(new string[] { Path });
        }

        public override string GetTaskHash()
        {
            return Path;
        }
    }
}
