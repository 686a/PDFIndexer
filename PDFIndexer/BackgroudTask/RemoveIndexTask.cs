using PDFIndexer.BackgroundTask;
using PDFIndexer.SearchEngine;
using PDFIndexer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFIndexer.BackgroudTask
{
    internal class RemoveIndexTask : AbstractTask
    {
        private static readonly Properties.Settings AppSettings = Properties.Settings.Default;

        public override string Name => "인덱스 삭제";
        public override string Description => $"{Path.Replace(AppSettings.BasePath, "")}";

        private string Path;

        public RemoveIndexTask(string path)
        {
            Path = path;
        }

        public override void Run()
        {
            new Indexer(SearchEngineContext.Provider)
                .RemoveIndexAllPages(Path);
        }

        public override string GetTaskHash()
        {
            return Path;
        }
    }
}
