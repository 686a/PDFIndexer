using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3
{
    internal class FileWatcher
    {
        public FileWatcher(string path)
        {
            var watcher = new FileSystemWatcher(path);

            watcher.NotifyFilter = NotifyFilters.FileName
                | NotifyFilters.Size
                | NotifyFilters.LastWrite
                | NotifyFilters.DirectoryName;

            watcher.Filter = "*.pdf";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }
    }
}
