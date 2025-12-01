using OpenCvSharp;
using Sdcb.PaddleInference;
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Local;
using System;
using System.Diagnostics;

namespace PDFIndexerOCR
{
    internal class Paddle
    {
        //#if DEBUG
        //        private const bool debug = true;
        //#else
        //        private const bool debug = false;
        //#endif

        FullOcrModel model = LocalFullModels.KoreanV4;
        
        Action<PaddleConfig> device;

        private PaddleOcrAll OCRInstance;

        public Paddle()
        {
            int useCpuThreads = 1;
            switch (Process.GetCurrentProcess().PriorityClass)
            {
                case ProcessPriorityClass.Idle:
                    useCpuThreads = 1;
                    break;
                case ProcessPriorityClass.BelowNormal:
                    useCpuThreads = 2;
                    break;
                case ProcessPriorityClass.Normal:
                    useCpuThreads = Environment.ProcessorCount / 2;
                    break;
                case ProcessPriorityClass.AboveNormal:
                case ProcessPriorityClass.High:
                case ProcessPriorityClass.RealTime:
                    useCpuThreads = 0;
                    break;
            }

            device = PaddleDevice.Onnx(cpuMathThreadCount: useCpuThreads, glogEnabled: false);

            OCRInstance = new PaddleOcrAll(model, device)
            {
                AllowRotateDetection = true,
            };
        }

        public PaddleOcrResult OCR(byte[] image)
        {
            using (Mat src = Cv2.ImDecode(image, ImreadModes.Color))
            {
                PaddleOcrResult result = OCRInstance.Run(src);

                return result;
            }
        }
    }
}
