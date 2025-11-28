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
    internal class IndexTask : AbstractTask
    {
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
