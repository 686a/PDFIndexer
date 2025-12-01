using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using PDFIndexer.Journal;
using PDFIndexer.Models.Database;
using PDFIndexer.Services;
using PDFIndexerShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using UglyToad.PdfPig;

namespace PDFIndexer.BackgroundTask
{
    internal class OCRTask : AbstractTask
    {
        private static readonly Properties.Settings AppSettings = Properties.Settings.Default;

        private static bool StopSignalReceived = false;

        private static Process OCRProcess;
        private static Thread IPCThread;
        private static StreamReader Reader;
        private static StreamWriter Writer;
        private static Queue<OCRTask> InternalQueue = new Queue<OCRTask>(); // 작업 내부 큐

        public override string Name => "OCR";
        public override string Description => $"{Path.Replace(AppSettings.BasePath, "").Replace(System.IO.Path.DirectorySeparatorChar, '/')} - {Page} 페이지";

        private string Path;
        private int Page;
        private bool IsLastPage = false;
        private Queue<byte[]> Images;
        private bool Done = false;
        private bool FailedOnce = false;

        public OCRTask(string path, int page, bool isLastPage = false)
        {
            Path = path;
            Page = page;
            IsLastPage = isLastPage;
            Images = new Queue<byte[]> { };

            if (!Program.Disposing) Setup();
        }

        public static void Setup()
        {
            if (!AppSettings.OCREnabled) return;

            if (OCRProcess != null) return;
            //if (!OCRProcess.HasExited) return;

            string priority = "idle";
            switch (AppSettings.OCRCPUPriority)
            {
                case 0:
                    priority = "idle";
                    break;
                case 1:
                    priority = "low";
                    break;
                case 2:
                    priority = "normal";
                    break;
                case 3:
                    priority = "high";
                    break;
            }

            var ocrProcessStartInfo = new ProcessStartInfo()
            {
                FileName = "PDFIndexerOCR",
                Arguments = $"--priority {priority}",
                UseShellExecute = false,
#if DEBUG
                CreateNoWindow = false,
#else
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
#endif
            };

            OCRProcess = Process.Start(ocrProcessStartInfo);

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
                while (!StopSignalReceived)
                {
                    if (!AppSettings.OCREnabled) break;

                    OCRTask task = null;
                    var Client = new NamedPipeClientStream(".", "PDFIndexerOCR", PipeDirection.InOut);

                    try
                    {
                        Client.Connect(30000);

                        Reader = new StreamReader(Client);
                        Writer = new StreamWriter(Client);

                        while (Client.IsConnected && !StopSignalReceived)
                        {
                            try
                            {
                                task = InternalQueue.Dequeue();
                            }
                            catch (InvalidOperationException)
                            {
                                task = null;

                                // 큐 Empty 패널티 부여
                                // 큐가 비었다면 다음에도 비어있을 가능성이 높음.
#if DEBUG
                                Thread.Sleep(1000);
#else
                                Thread.Sleep(30 * 1000);
#endif

                                continue;
                            }

                            // OCR 미활성화
                            if (!AppSettings.OCREnabled) continue;

                            Logger.Write($"[OCRTask-IPC] {task.Path}/{task.Page} Start");

                            var dbCollection = DBContext.DB.GetCollection<IndexedDocument>("indexed");
                            var dbItem = dbCollection.FindOne(LiteDB.Query.And(LiteDB.Query.EQ("Path", task.Path), LiteDB.Query.EQ("Page", task.Page)));
                            if (dbItem == null)
                            {
                                task.Done = true;
                                continue;
                            }

                            // 이미지가 없으면 스킵
                            if (task.Images != null && task.Images.Count > 0)
                            {
                                string result = "";

                                foreach (var image in task.Images)
                                {
                                    if (image == null) continue;

                                    /**
                                        * 전송 데이터
                                        * | 헤더 (4 bytes int) | body (n bytes)        |
                                        * | ----------------- | --------------------- |
                                        * | 데이터 길이         | 실 이미지 데이터 n bytes |
                                        */
                                    byte[] header = BitConverter.GetBytes(image.Length);
                                    Client.Write(header, 0, header.Length);
                                    Client.Write(image, 0, image.Length);
                                    Client.Flush();

                                    string data = Reader.ReadLine();
                                    if (data != null)
                                    {
                                        OCRPipeResponse response = PipeResponse.FromJSON<OCRPipeResponse>(data);
                                        if (response.status != 200)
                                        {
                                            throw new InvalidDataException("OCR error");
                                        }

                                        result += response.Text + "\n";
                                    }
                                }

                                if (result.Length > 0)
                                {
                                    Logger.Write($"[OCRTask-IPC] OCR Done {task.Path}/{task.Page} : Length {result.Length}");
                                }

                                var indexWriter = SearchEngineContext.Provider.GetIndexWriter();

                                // 기존 인덱스 제거
                                BooleanQuery deleteQuery = new BooleanQuery
                                {
                                    { new TermQuery(new Term("path", task.Path)), Occur.MUST },
                                    { new TermQuery(new Term("isOCRData", "1")), Occur.MUST },
                                };
                                if (task.IsLastPage)
                                    deleteQuery.Add(NumericRangeQuery.NewInt32Range("page", task.Page, null, true, false), Occur.MUST);
                                else
                                    deleteQuery.Add(new TermQuery(new Term("page", task.Page.ToString())), Occur.MUST);
                                indexWriter.DeleteDocuments(deleteQuery);

                                // 인덱스 저장
                                var filename = (task.Path.Split('\\').LastOrDefault() ?? task.Path).Replace(".pdf", "");
                                Document doc = new Document
                                {
                                    new StringField("title", filename, Field.Store.YES),
                                    new StringField("path", task.Path, Field.Store.YES),
                                    new Int32Field("page", task.Page, Field.Store.YES),
                                    new TextField("content", result, Field.Store.YES),
                                    new StringField("isOCRData", "1", Field.Store.YES),
                                };

                                indexWriter.AddDocument(doc);
                                indexWriter.Commit();
                                SearchEngineContext.Provider.MarkAsDirty();
                            }

                            // DB 업데이트
                            dbItem.OCRDone = true;
                            dbCollection.Update(dbItem);

                            Logger.Write($"[OCRTask-IPC] {task.Path}/{task.Page} Done");

                            Thread.Sleep(500);

                            // 완료 표시
                            task.Done = true;
                        }
                    }
                    catch (Exception e)
                    {
                        // https://learn.microsoft.com/en-us/dotnet/api/system.io.pipes.pipestream.write?view=netframework-4.7.2
                        // Pipe broken or pipe closed
                        if (e is ObjectDisposedException || e is IOException)
                        {
                            Logger.Write(JournalLevel.Error, "[OCRTask-IPC] Pipe closed or broken");

                            if (task != null && !task.Done && !task.FailedOnce)
                            {
                                // Mask as failed once
                                task.FailedOnce = true;

                                // Re-enqueue task
                                InternalQueue.Enqueue(task);

                                Logger.Write($"[OCRTask-IPC] Re-enqueued task {task.Path}/{task.Page}");
                            }
                        }
                        else if (e is TimeoutException)
                        {
                            // Pipe connect timed out
                            Logger.Write(JournalLevel.Error, "[OCRTask-IPC] Pipe connect timeout");

                            // 연결 타임아웃의 경우 대부분 프로세스가 죽었을 경우임
                            // 일단 타임아웃이 되면, 프로세스 다시시작
                            if (OCRProcess != null && !OCRProcess.HasExited) OCRProcess.Kill();
                            OCRProcess = Process.Start(ocrProcessStartInfo);
                            // 프로세스 시작 대기
                            Thread.Sleep(1000);

                            if (task != null && !task.Done)
                            {
                                // Re-enqueue task
                                InternalQueue.Enqueue(task);

                                Logger.Write($"[OCRTask-IPC] Re-enqueued task {task.Path}/{task.Page}");
                            }
                        }
                        else if (e is InvalidDataException)
                        {
                            Logger.Write(JournalLevel.Error, "[OCRTask-IPC] OCR error");

                            if (task != null && !task.Done && !task.FailedOnce)
                            {
                                task.FailedOnce = true;

                                // Re-enqueue task
                                InternalQueue.Enqueue(task);

                                Logger.Write($"[OCRTask-IPC] Re-enqueued task {task.Path}/{task.Page}");
                            }
                        }
                        else
                        {
                            // 기타 에러
                            Logger.Write(JournalLevel.Error, "[OCRTask-IPC] Error on IPC thread:");
                            Logger.Write(JournalLevel.Error, e.ToString());
                        }

                        // 에러 패널티
                        Thread.Sleep(10000);
                    } finally
                    {
                        if (Client.IsConnected)
                        {
                            Client.Close();
                            Client.Dispose();
                        }
                    }
                }
            });

            Logger.Write("[OCRTask] IPC 스레드 시작");
            IPCThread.Start();
        }

        public static void Stop()
        {
            try
            {
                StopSignalReceived = true;

                if (IPCThread != null) IPCThread.Abort();

                if (OCRProcess != null && !OCRProcess.HasExited) OCRProcess.Kill();
            } catch { }
        }

        public override void Run()
        {
            // 해당 PDF 페이지의 이미지 추출해서 queue에 삽입
            // 모든 페이지 이미지 enqueue > 페이지 단위로 이미지 enqueue
            // 매 페이지마다 디스크 IO 작업이 일어나지만,
            // 메모리 절약을 우선으로 페이지 단위로 읽음

            if (!AppSettings.OCREnabled) return;

            if (!File.Exists(Path)) return;

            Logger.Write($"[OCRTask] {Path}/{Page} Start");

            var waitThread = new Thread(() =>
            {
                while (!Done && !Program.Disposing) Thread.Sleep(300);
            });

            using (var pdf = PdfDocument.Open(Path))
            {
                var page = pdf.GetPage(Page);

                var images = page.GetImages();
                foreach (var image in images)
                {
                    image.TryGetPng(out var bytes);
                    // 최소 이미지 크기
                    if (bytes != null && bytes.Length >= 1024)
                    {
                        Images.Enqueue(bytes);
                    }
                }
            }

            if (Images.Count > 0)
            {
                InternalQueue.Enqueue(this);

                waitThread.Start();
                waitThread.Join();
            }
            else
            {
                var dbCollection = DBContext.DB.GetCollection<IndexedDocument>("indexed");
                var dbItem = dbCollection.FindOne(LiteDB.Query.And(LiteDB.Query.EQ("Path", Path), LiteDB.Query.EQ("Page", Page)));
                if (dbItem != null)
                {
                    dbItem.OCRDone = true;
                    dbCollection.Update(dbItem);
                }

                Done = true;
            }

            Logger.Write($"[OCRTask] {Path}/{Page} Done");
        }

        public override string GetTaskHash()
        {
            return $"{Path}/{Page}";
        }
    }
}
