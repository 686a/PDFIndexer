using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDFIndexer.Journal;
using PDFIndexer.SearchEngine;
using PDFIndexer.SetupWizard;

namespace PDFIndexer
{
    internal static class Program
    {
        private static readonly Properties.Settings AppSettings = Properties.Settings.Default;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logger.Write(JournalLevel.Info, "프로그램 진입점");

            /**
             * 초기 실행 -> run setup wizard
             * 기사용자 no or broken configuration -> run setup wizard
             * 기사용자 -> 일반 실행
             */

            SetupDefaultSettings();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 초기 실행 시 Setup wizard 실행
            if (!AppSettings.DoneSetupWizard)
            {
                Logger.Write($"Settings.DoneSetupWizard : false --> Setup wizard 실행");
                Application.Run(new SetupWizardForm());

                return;
            }

            LuceneProvider luceneProvider = new LuceneProvider(AppSettings.BasePath);
            luceneProvider.Initialize();

            Logger.Write($"메인 UI 실행");
            Application.Run(new Form1(luceneProvider));

            luceneProvider.Dispose();
        }

        // 설정 기본값 저장
        private static void SetupDefaultSettings()
        {
            if (string.IsNullOrWhiteSpace(AppSettings.BasePath))
            {
                Logger.Write($"BasePath가 비어있음 : {AppSettings.BasePath}");

                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                AppSettings.BasePath = path;
                AppSettings.Save();

                Logger.Write($"BasePath 기본값 설정 : {path}");
            }
        }
    }
}
