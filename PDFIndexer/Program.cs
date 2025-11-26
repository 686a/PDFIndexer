using PDFIndexer.BackgroundTask;
using PDFIndexer.Journal;
using PDFIndexer.SearchEngine;
using PDFIndexer.SetupWizard;
using PDFIndexerShared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFIndexer
{
    internal static class Program
    {
        private static bool _Disposing = false;
        public static bool Disposing { get { return _Disposing; } }

        private static readonly Properties.Settings AppSettings = Properties.Settings.Default;

        private static LuceneProvider LuceneProvider;

        private static TaskManager TaskManager;

        private static FileWatcher FileWatcher;

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

            // 베이스 디렉토리 생성
            string appDataPath = Path.Combine(AppSettings.BasePath, ".pdfindexer");
            Directory.CreateDirectory(appDataPath);

            // 작업 관리자
            TaskManager = new TaskManager();
            TaskManager.Start();

            // 데이터베이스
            new DBContext();

            // 인덱서
            LuceneProvider = new LuceneProvider(appDataPath);
            LuceneProvider.Initialize();

            // 파일 감시기
            FileWatcher = new FileWatcher(AppSettings.BasePath);

            Logger.Write($"메인 UI 실행");
            Application.Run(new Form1(LuceneProvider));

            // 정리
            Cleanup();
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

        private static void Cleanup()
        {
            _Disposing = true;

            Logger.Write("프로세스 정리 - FileWatcher");
            FileWatcher.Dispose();

            Logger.Write("프로세스 정리 - TaskManager");
            TaskManager.Stop();

            Logger.Write("프로세스 정리 - LuceneProvider");
            LuceneProvider.Dispose();

            Logger.Write("프로세스 정리 - DBContext");
            DBContext.Dispose();

            Logger.Write("프로세스 정리 끝");
        }
    }
}
