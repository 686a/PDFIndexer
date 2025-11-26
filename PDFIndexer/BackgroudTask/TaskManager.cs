using PDFIndexer.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PDFIndexer.BackgroundTask
{
    internal class TaskManager
    {
        private static Queue<AbstractTask> Tasks;
        private static HashSet<KeyValuePair<string, string>> TaskHashes;

        private Thread TaskThread;
        private bool NeedToStop = false;

#if DEBUG
        private static readonly int EmptyTaskPenalty = 3000;
        private static readonly int DelayPerTask = 1000;
#else
        private static readonly int EmptyTaskPenalty = 30 * 1000;
        private static readonly int DelayPerTask = 3000;
#endif

        public TaskManager()
        {
            Tasks = new Queue<AbstractTask>();
            TaskHashes = new HashSet<KeyValuePair<string, string>>();
            TaskThread = new Thread(TaskRunner);
        }

        public void Start()
        {
            NeedToStop = false;
            TaskThread.Start();
        }

        public void Stop()
        {
            TaskThread.Abort();

            OCRTask.Stop();
        }

        private void TaskRunner()
        {
            while (!NeedToStop)
            {
                // Empty task queue panelty
                if (Tasks.Count == 0)
                {
                    Thread.Sleep(EmptyTaskPenalty);
                    continue;
                }

                var task = Tasks.Dequeue();
                var hash = new KeyValuePair<string, string>(task.ToString(), task.GetTaskHash());

                // 작업 실행
                Logger.Write($"[TaskManager] Task started: {hash.Key}/{hash.Value}");
                task.Run();
                Logger.Write($"[TaskManager] Task done: {hash.Key}/{hash.Value}");

                // 작업 종료 후 해시 목록에서 제거
                TaskHashes.Remove(hash);

                Thread.Sleep(DelayPerTask);
            }
        }

        public static void Enqueue(AbstractTask task)
        {
            var hash = new KeyValuePair<string, string>(task.ToString(), task.GetTaskHash());
            if (TaskHashes.Contains(hash)) return;

            Tasks.Enqueue(task);
            TaskHashes.Add(hash);

            // Logger.Write($"[TaskManager] Task enqueue: {hash.Key}/{hash.Value}");
        }

        public static bool IsExists(string type, string taskHash)
        {
            var hash = new KeyValuePair<string, string>(type, taskHash);
            return TaskHashes.Contains(hash);
        }
    }
}
