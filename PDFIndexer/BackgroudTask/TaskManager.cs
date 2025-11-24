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
            TaskThread = new Thread(TaskRunner);
        }

        public void Start()
        {
            NeedToStop = false;
            TaskThread.Start();
        }

        public void Stop()
        {
            NeedToStop = true;
            TaskThread.Join();
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
                task.Run();

                Thread.Sleep(DelayPerTask);
            }
        }

        public static void Enqueue(AbstractTask task)
        {
            Tasks.Enqueue(task);
        }
    }
}
