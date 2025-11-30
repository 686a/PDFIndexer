using PDFIndexer.BackgroudTask;
using PDFIndexer.BackgroundTask;
using PDFIndexer.Journal;
using System.IO;

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

            EnqueueRemoveIndexTask(e.OldFullPath);
            EnqueueIndexTask(e.FullPath);
        }

        private void FSWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Logger.Write($"[FSWatcher] Deleted: {e.FullPath}");

            EnqueueRemoveIndexTask(e.FullPath);
        }

        private void FSWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Logger.Write($"[FSWatcher] Created: {e.FullPath}");

            EnqueueIndexTask(e.FullPath);
        }

        private void FSWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Logger.Write($"[FSWatcher] Changed: {e.FullPath}");

            EnqueueIndexTask(e.FullPath);
        }

        private void EnqueueIndexTask(string path)
        {
            var task = new IndexTask(path);
            TaskManager.Enqueue(task, true);
        }

        private void EnqueueRemoveIndexTask(string path)
        {
            var task = new RemoveIndexTask(path);
            TaskManager.Enqueue(task, true);
        }
    }
}
