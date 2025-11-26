using PDFIndexer.BackgroundTask;
using PDFIndexer.SearchEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFIndexer.BackgroudTask
{
    internal class IndexTask : AbstractTask
    {
        private LuceneProvider Provider;
        private string Path;

        private Indexer Indexer;

        public IndexTask(LuceneProvider provider, string path)
        {
            Provider = provider;
            Path = path;

            Indexer = new Indexer(Provider);
        }

        public override void Run()
        {
            Indexer.IndexPdfs(new string[] { Path });
            Provider.Initialize();
        }

        public override string GetTaskHash()
        {
            return Path;
        }
    }
}
