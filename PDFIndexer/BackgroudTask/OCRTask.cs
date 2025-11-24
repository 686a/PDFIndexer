using Lucene.Net.Documents;
using PDFIndexerShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace PDFIndexer.BackgroundTask
{
    internal class OCRTask : AbstractTask
    {
        private static Process OCRProcess;
        private static Thread IPCThread;
        private static StreamReader Reader;
        private static StreamWriter Writer;
        private static Queue<OCRTask> InternalQueue = new Queue<OCRTask>(); // 작업 내부 큐

        private string Path;
        private int Page;
        private byte[] Image = null;

        public OCRTask(string path, int page)
        {
            Path = path;
            Page = page;

            Setup();
        }

        public static void Setup()
        {
            if (OCRProcess != null) return;
            //if (!OCRProcess.HasExited) return;

            var startInfo = new ProcessStartInfo()
            {
                FileName = "PDFIndexerOCR",
                UseShellExecute = false,
#if DEBUG
                CreateNoWindow = false,
#else
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
#endif
            };

            OCRProcess = Process.Start(startInfo);

            // 기존 스레드 정지
            if (IPCThread != null)
            {
                try
                {
                    IPCThread.Abort();
                }
                catch (Exception) { }
            }

            IPCThread = new Thread(() =>
            {
                using (var Client = new NamedPipeClientStream(".", "PDFIndexerOCR", PipeDirection.InOut))
                {
                    Client.Connect(5000);

                    Reader = new StreamReader(Client);
                    Writer = new StreamWriter(Client);

                    while (Client.IsConnected)
                    {
                        OCRTask task;
                        try
                        {
                            task = InternalQueue.Dequeue();
                        } catch (InvalidOperationException)
                        {
                            // 큐 Empty 패널티 부여
                            // 큐가 비었다면 다음에도 비어있을 가능성이 높음.
                            //Thread.Sleep(30 * 1000);
                            Thread.Sleep(1000);

                            continue;
                        }

                        // 이미지가 없으면 스킵
                        if (task.Image == null) continue;
                        if (task.Image.Length == 0) continue;

                        /**
                            * 전송 데이터
                            * | 헤더 (4 bytes int) | body (n bytes)        |
                            * | ----------------- | --------------------- |
                            * | 데이터 길이         | 실 이미지 데이터 n bytes |
                            */
                        byte[] header = BitConverter.GetBytes(task.Image.Length);
                        Client.Write(header, 0, header.Length);
                        Client.Write(task.Image.ToArray(), 0, task.Image.Length);
                        Client.Flush();

                        string result = Reader.ReadLine();
                        OCRPipeResponse response = PipeResponse.FromJSON<OCRPipeResponse>(result);

                        Document doc = new Document
                        {
                            new StringField("path", task.Path, Field.Store.YES),
                            new Int32Field("page", task.Page, Field.Store.YES),
                            new TextField("content", response.Text, Field.Store.YES),
                            new StringField("isOCRData", "1", Field.Store.YES),
                        };
                        // TODO: Write to index

                        Thread.Sleep(5000);
                    }
                }
            });

            IPCThread.Start();
        }

        public override void Run()
        {
            // 해당 PDF 페이지의 이미지 추출해서 queue에 삽입
            // 모든 페이지 이미지 enqueue > 페이지 단위로 이미지 enqueue
            // 매 페이지마다 디스크 IO 작업이 일어나지만,
            // 메모리 절약을 우선으로 페이지 단위로 읽음

            using (var pdf = PdfDocument.Open(Path))
            {
                var page = pdf.GetPage(Page);

                foreach (var image in page.GetImages())
                {
                    image.TryGetPng(out var bytes);
                    Image = bytes;
                    InternalQueue.Enqueue(this);
                }
            }
        }
    }
}
