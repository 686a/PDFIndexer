using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFIndexer
{
    internal class FileWatcher
    {
        private FileSystemWatcher FSWatcher;

        public FileWatcher(string path)
        {
            var watcher = new FileSystemWatcher(path);

            watcher.NotifyFilter = NotifyFilters.FileName
                | NotifyFilters.Size
                | NotifyFilters.LastWrite
                | NotifyFilters.DirectoryName;

            FSWatcher.Filter = "*.pdf";
            FSWatcher.IncludeSubdirectories = true;
            FSWatcher.EnableRaisingEvents = true;

        }

        public void Dispose()
        {
            FSWatcher.Dispose();
        }
    }
}
