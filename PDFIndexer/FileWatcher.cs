using PDFIndexer.BackgroudTask;
using PDFIndexer.BackgroundTask;
using PDFIndexer.Journal;
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
            FSWatcher = new FileSystemWatcher(path);

            FSWatcher.NotifyFilter = NotifyFilters.FileName
                | NotifyFilters.Size
                | NotifyFilters.LastWrite
                | NotifyFilters.DirectoryName;

            FSWatcher.Filter = "*.pdf";
            FSWatcher.IncludeSubdirectories = true;
            FSWatcher.EnableRaisingEvents = true;

            FSWatcher.Changed += FSWatcher_Changed;
            FSWatcher.Created += FSWatcher_Created;
            FSWatcher.Deleted += FSWatcher_Deleted;
            FSWatcher.Renamed += FSWatcher_Renamed;
        }

        public void Dispose()
        {
            FSWatcher?.Dispose();
        }

        private void FSWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            Logger.Write($"[FSWatcher] Renamed: {e.OldFullPath} -> {e.FullPath}");

            // TODO: Remove old index
            EnqueueTask(e.FullPath);
        }

        private void FSWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Logger.Write($"[FSWatcher] Deleted: {e.FullPath}");

            // TODO: Remove index
        }

        private void FSWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Logger.Write($"[FSWatcher] Created: {e.FullPath}");

            EnqueueTask(e.FullPath);
        }

        private void FSWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Logger.Write($"[FSWatcher] Changed: {e.FullPath}");

            EnqueueTask(e.FullPath);
        }

        private void EnqueueTask(string path)
        {
            var task = new IndexTask(path);
            TaskManager.Enqueue(task);
        }
    }
}
