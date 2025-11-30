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

        public Paddle()
        {
            int useCpuThreads = 4;
            switch (Process.GetCurrentProcess().PriorityClass)
            {
                case ProcessPriorityClass.Idle:
                    useCpuThreads = 4;
                    break;
                case ProcessPriorityClass.Normal:
                case ProcessPriorityClass.BelowNormal:
                    useCpuThreads = Environment.ProcessorCount / 2;
                    break;
                case ProcessPriorityClass.AboveNormal:
                case ProcessPriorityClass.High:
                case ProcessPriorityClass.RealTime:
                    useCpuThreads = 0;
                    break;
            }
            device = PaddleDevice.Onnx(cpuMathThreadCount: useCpuThreads, glogEnabled: false);
        }

        public PaddleOcrResult OCR(byte[] image)
        {
            using (PaddleOcrAll all = new PaddleOcrAll(model, device)
            {
                // 요거 켜면 정확도 낮아짐
                //AllowRotateDetection = true,
                //Enable180Classification = true,
            })
            {
                using (Mat src = Cv2.ImDecode(image, ImreadModes.Color))
                {
                    PaddleOcrResult result = all.Run(src);

                    return result;
                }
            }
        }
    }
}
