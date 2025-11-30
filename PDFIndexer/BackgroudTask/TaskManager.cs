using PDFIndexer.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static PDFIndexer.BackgroundTask.TaskManager;

namespace PDFIndexer.BackgroundTask
{
    internal class TaskManager
    {
        private static Queue<AbstractTask> Tasks;
        private static Queue<AbstractTask> PriorityTasks;
        private static HashSet<KeyValuePair<string, string>> TaskHashes;
        private static AbstractTask CurrentTask;

        private Thread TaskThread;
        private bool NeedToStop = false;
        private bool IsLastEmpty = true;
        private static int _TasksDone = 0;
        public static int TasksDone { get { return _TasksDone; } }

#if DEBUG
        private static readonly int EmptyTaskPenalty = 3000;
        private static readonly int DelayPerTask = 1000;
#else
        private static readonly int EmptyTaskPenalty = 30 * 1000;
        private static readonly int DelayPerTask = 3000;
#endif

        public delegate void TaskStart(string name, string description);
        public static event TaskStart OnTaskStart;

        public delegate void TaskDone();
        public static event TaskDone OnTaskDone;

        public TaskManager()
        {
            Tasks = new Queue<AbstractTask>();
            PriorityTasks = new Queue<AbstractTask>();
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
                if (Tasks.Count == 0 && PriorityTasks.Count == 0)
                {
                    if (!IsLastEmpty)
                    {
                        IsLastEmpty = true;
                        OnTaskDone?.Invoke();
                    }

                    Thread.Sleep(EmptyTaskPenalty);
                    continue;
                }

                if (IsLastEmpty)
                {
                    IsLastEmpty = false;
                    _TasksDone = 0;
                }

                if (PriorityTasks.Count > 0)
                {
                    CurrentTask = PriorityTasks.Dequeue();
                } else
                {
                    CurrentTask = Tasks.Dequeue();
                }

                var hash = new KeyValuePair<string, string>(CurrentTask.ToString(), CurrentTask.GetTaskHash());

                // 작업 실행
                Logger.Write($"[TaskManager] Task started: {hash.Key}/{hash.Value}");
                OnTaskStart?.Invoke(CurrentTask.Name, CurrentTask.Description);
                CurrentTask.Run();
                Logger.Write($"[TaskManager] Task done: {hash.Key}/{hash.Value}");

                // 작업 종료 후 해시 목록에서 제거
                TaskHashes.Remove(hash);
                _TasksDone++;

                Thread.Sleep(DelayPerTask);
            }
        }

        public static bool Enqueue(AbstractTask task, bool priority = false)
        {
            var hash = new KeyValuePair<string, string>(task.ToString(), task.GetTaskHash());
            if (TaskHashes.Contains(hash)) return false;

            if (priority) PriorityTasks.Enqueue(task);
            else Tasks.Enqueue(task);
            TaskHashes.Add(hash);

            // Logger.Write($"[TaskManager] Task enqueue: {hash.Key}/{hash.Value}");

            return true;
        }

        public static bool IsExists(string type, string taskHash)
        {
            var hash = new KeyValuePair<string, string>(type, taskHash);
            return TaskHashes.Contains(hash);
        }

        public static KeyValuePair<string, string> GetCurrentTask()
        {
            if (CurrentTask == null) return new KeyValuePair<string, string>(null, null);

            return new KeyValuePair<string, string>(CurrentTask.Name, CurrentTask.Description);
        }

        public static int GetRemainTasks()
        {
            return TaskHashes.Count;
        }
    }
}
