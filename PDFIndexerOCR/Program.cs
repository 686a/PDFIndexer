using PDFIndexerShared;
using Sdcb.PaddleOCR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PDFIndexerOCR
{
    internal class Program
    {
        private static readonly Mutex Mutex = new Mutex(initiallyOwned: false, name: "com.github.686a.PDFIndexer.OCR");

        private static bool SingleMode = false;

        private static string Path;

        private static Paddle OCRProvider;

        private static Thread PipeServerThread;

        static void Main(string[] args)
        {
            Console.WriteLine("PDFIndexerOCR\n");

            ParseArgs(args);

            OCRProvider = new Paddle();

            if (!SingleMode)
            {
                if (!Mutex.WaitOne(TimeSpan.Zero, true)) return;

                MainProcessWatcher();

                PipeServerThread = new Thread(StartPipeServer);
                PipeServerThread.Start();

                Mutex.ReleaseMutex();
            } else
            {
                OCRSingle();
            }
        }

        private static void ParseArgs(string[] args)
        {
            var priority = ProcessPriorityClass.Idle;

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                string nextArg = null;
                if (args.Length > i + 1) nextArg = args[i + 1];

                switch (arg)
                {
                    case "-i":
                    case "--input":
                        if (nextArg == null) throw new ArgumentException($"Invalid value for {arg}");
                        SingleMode = true;
                        Path = nextArg;
                        break;

                    case "-p":
                    case "--priority":
                        if (nextArg == null) throw new ArgumentException($"Invalid value for {arg}");

                        switch (nextArg.ToLower())
                        {
                            case "high":
                                priority = ProcessPriorityClass.AboveNormal;
                                break;
                            case "normal":
                                priority = ProcessPriorityClass.Normal;
                                break;
                            case "low":
                                priority = ProcessPriorityClass.BelowNormal;
                                break;
                            case "idle":
                                priority = ProcessPriorityClass.Idle;
                                break;
                            default:
                                throw new ArgumentException($"Invalid value for {arg}");
                        }
                        break;
                }
            }

            Process.GetCurrentProcess().PriorityClass = priority;
        }

        private static void StartPipeServer()
        {
            for (; ; )
            {
                using (var server = new NamedPipeServerStream("PDFIndexerOCR", PipeDirection.InOut))
                {
                    Console.WriteLine("[Server] 클라이언트 연결 대기중");

                    server.WaitForConnection();

                    Console.WriteLine("[Server] Client Connected");

                    using (var reader = new StreamReader(server))
                    using (var writer = new StreamWriter(server) { AutoFlush = true })
                    {
                        while (server.IsConnected)
                        {
                            try
                            {
                                /**
                                  * 전송 데이터
                                  * | 헤더 (4 bytes int) | body (n bytes)        |
                                  * | ----------------- | --------------------- |
                                  * | 데이터 길이         | 실 이미지 데이터 n bytes |
                                  */

                                // 헤더 읽기
                                byte[] lengthBuffer = new byte[4];
                                int bytesRead = server.Read(lengthBuffer, 0, 4);

                                // 잘못된 입력 --> 연결 종료
                                if (bytesRead == 0) break;

                                int dataLength = BitConverter.ToInt32(lengthBuffer, 0);

                                // 바디 읽기
                                byte[] imageBuffer = new byte[dataLength];
                                int totalRead = 0;

                                // 모두 읽기
                                while (totalRead < dataLength)
                                {
                                    int read = server.Read(imageBuffer, totalRead, dataLength - totalRead);

                                    // 읽은 바이트 수가 0이면 종료
                                    // 다 읽었거나, 들어온 데이터가 없거나 아님 클라이언트가 보내지 않았던가 --> 잘못된 입력
                                    if (read == 0) break;

                                    totalRead += read;
                                }

                                Console.WriteLine($"[OCR] -------------------------------------------");
                                Console.WriteLine($"[OCR] Start OCR. Received {totalRead} bytes.");

                                var stopwatch = new Stopwatch();
                                stopwatch.Start();

                                List<OCRRegion> regions = new List<OCRRegion>();

                                PaddleOcrResult result = OCRProvider.OCR(imageBuffer);
                                foreach (PaddleOcrResultRegion region in result.Regions)
                                {
                                    regions.Add(new OCRRegion()
                                    {
                                        Text = region.Text,
                                        Score = region.Score,

                                        CenterX = (int)region.Rect.Center.X,
                                        CenterY = (int)region.Rect.Center.Y,
                                        Width = (int)region.Rect.Size.Width,
                                        Height = (int)region.Rect.Size.Height,
                                        Angle = region.Rect.Angle,
                                    });
                                }

                                var res = new OCRPipeResponse(result.Text, regions.ToArray());
                                var response = PipeResponse.ToJSON(res);

                                stopwatch.Stop();
                                Console.WriteLine($"[OCR] Result: {result.Text.Substring(0, 20).Replace("\n", " ")}{(result.Text.Length > 20 ? "..." : "")} (length: {result.Text.Length})");
                                Console.WriteLine($"[OCR] Elapsed: {stopwatch.Elapsed}");

                                writer.WriteLine(response);

                                Console.WriteLine($"[OCR] -------------------------------------------");
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    // 클라이언트에 에러 전송
                                    var response = new OCRPipeResponse(null)
                                    {
                                        status = 500,
                                    };
                                    var json = PipeResponse.ToJSON(response);

                                    writer.WriteLine(json);
                                }
                                catch { }

                                Console.Error.WriteLine($"[OCR] Failed to OCR:");
                                Console.Error.WriteLine(e.ToString());
                            }
                        }
                    }

                    Debug.WriteLine("[Server] Client Disconnected");
                }
            }
        }

        private static async void MainProcessWatcher()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (Process.GetProcessesByName("PDFIndexer").Length == 0)
                    {
                        // Kill
                        Console.WriteLine("No main process detected. Exit");

                        PipeServerThread?.Abort();

                        Environment.Exit(0);
                    }

#if DEBUG
                    Thread.Sleep(1000);
#else
                    Thread.Sleep(5000);
#endif
                }
            });
        }

        private static void OCRSingle()
        {
            // TODO:
        }
    }
}
