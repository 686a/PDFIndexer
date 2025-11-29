using Microsoft.Win32;
using PDFIndexer.BackgroundTask;
using PDFIndexer.Journal;
using PDFIndexer.SearchEngine;
using PDFIndexer.Services;
using PDFIndexer.SetupWizard;
using PDFIndexerShared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFIndexer
{
    internal static class Program
    {
        private static bool _Disposing = false;
        public static bool Disposing { get { return _Disposing; } }
        private static readonly Mutex Mutex = new Mutex(initiallyOwned: false, name: "com.github.686a.PDFIndexer.MainProcess");

        private static readonly Properties.Settings AppSettings = Properties.Settings.Default;

        private static LuceneProvider LuceneProvider;

        private static TaskManager TaskManager;

        private static FileWatcher FileWatcher;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool backgroundLaunch = false;
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    if (arg.ToLower() == "-b" || arg.ToLower() == "--background")
                    {
                        backgroundLaunch = true;
                    }

                    if (arg.ToLower() == "--install-startup")
                    {
                        InstallStartup(true);
                        return;
                    }

                    if (arg.ToLower() == "--uninstall-startup")
                    {
                        InstallStartup(false);
                        return;
                    }
                }
            }

            // 단일 인스턴스 허용
            if (!Mutex.WaitOne(TimeSpan.Zero, true)) return;

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
            var directory = Directory.CreateDirectory(appDataPath);
            directory.Attributes |= FileAttributes.Hidden;

            // 작업 관리자
            TaskManager = new TaskManager();
            TaskManager.Start();

            // 데이터베이스
            new DBContext();

            // 인덱서
            new SearchEngineContext(appDataPath);

            LuceneProvider = new LuceneProvider(appDataPath);
            LuceneProvider.Initialize();

            // 파일 감시기
            FileWatcher = new FileWatcher(AppSettings.BasePath);

            Logger.Write($"메인 UI 실행");
            Application.Run(new Form1(LuceneProvider, backgroundLaunch));

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

            Logger.Write("[1/4] 프로세스 정리 : FileWatcher");
            FileWatcher.Dispose();

            Logger.Write("[2/4] 프로세스 정리 - TaskManager");
            TaskManager.Stop();

            Logger.Write("[3/4] 프로세스 정리 - SearchEngineContext");
            SearchEngineContext.Dispose();

            Logger.Write("[4/4] 프로세스 정리 - DBContext");
            DBContext.Dispose();

            Logger.Write("프로세스 정리 완료");

            Mutex.ReleaseMutex();
        }

        private static void InstallStartup(bool install)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (key != null)
                {
                    if (install)
                        key.SetValue("PDFIndexer", $"\"{Assembly.GetExecutingAssembly().Location}\" -b");
                    else
                        key.DeleteValue("PDFIndexer");

                    key.Close();
                }
            } catch { }
        }
    }
}
