using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using PDFIndexer.Journal;
using static Lucene.Net.Util.Fst.Builder;
using static Lucene.Net.Util.Packed.PackedInt32s;
using Directory = System.IO.Directory;
using PDFIndexer.SearchEngine;
using PDFIndexer.Services;
using PDFIndexer.SettingsView;
using Microsoft.Toolkit.Uwp.Notifications;
using PDFIndexer.BackgroundTask;
using PDFIndexer.BackgroudTask;

namespace PDFIndexer
{
    public partial class Form1 : Form
    {
        private static readonly Properties.Settings AppSettings = Properties.Settings.Default;

        private string basePath
        {
            get { return AppSettings.BasePath; }
        }

        private readonly List<string> pdfs = new List<string>();

        // private Uri currentPdf;

        private LuceneProvider Provider = SearchEngineContext.Provider;
        private Indexer Indexer;
        private Searcher Searcher;

        private bool _IsIndexing = false;
        private bool IsIndexing
        {
            get { return _IsIndexing; }
            set
            {
                _IsIndexing = value;

                foreach (Control control in IndexLockControls)
                {
                    control.Enabled = !value;
                }
            }
        }

        // 인덱스 동안 잠굴 컨트롤 목록
        private Control[] IndexLockControls;

        PdfWebView pdfWebView;
        bool _WebViewIsDetached = false;
        bool WebViewIsDetached
        {
            get { return _WebViewIsDetached; }
            set
            {
                _WebViewIsDetached = value;
                if (value) DetachButton.Image = Properties.Resources.OpenInNewDownIcon;
                else DetachButton.Image = Properties.Resources.OpenInNewIcon;
            }
        }

        DuplicateManagerView duplicateManagerView;

        // true로 설정 시 CloseToTray 설정을 무시하고 창을 닫을 수 있도록 함.
        private bool forceQuit = false;

        private bool BackgroundMode = false;

        private bool ProgressPanelInUse = false;

        public Form1(LuceneProvider provider, bool backgroundMode = false)
        {
#if DEBUG
            new DebugForm().Show();
#endif

            BackgroundMode = backgroundMode;

            // TODO: Asynchronous loading
            Indexer = new Indexer(Provider);
            Searcher = new Searcher(Provider);

            InitializeComponent();

            if (!backgroundMode) WindowState = FormWindowState.Normal;

            noFileLabel.Location = new Point(
                (WebViewVirtualPanel.ClientSize.Width / 2) - (noFileLabel.ClientSize.Width / 2),
                (WebViewVirtualPanel.ClientSize.Height / 2) - (noFileLabel.ClientSize.Height / 2)
            );

            // 인덱스 동안 잠굴 컨트롤 목록
            IndexLockControls = new Control[]
            {
                // 최상단
                QueryInputBox,

                // 좌측
                SearchResultPanel,

                // 우측-상단
                OpenInNewWindowButton,
                DetachButton,

                // 우측-하단
                SettingsButton,
                IndexAllButton,
                IndexMissingButton,
                DuplicateManagerButton,
            };

            //BackColor = Color.White;
            //TransparencyKey = Color.White;

            pdfWebView = new PdfWebView();
            pdfWebView.FormClosing += PdfWebView_FormClosing;
            AttachWebView();

            duplicateManagerView = new DuplicateManagerView();

            //Indexer.CleanupIndexes(indexer);

            if (!LuceneProvider.NotIndexedYet)
            {
                TaskManager.Enqueue(new CheckMissingTask());
            }
        }

        #region 웹뷰 관련
        private void AttachWebView()
        {
            pdfWebView.TopLevel = false;
            pdfWebView.FormBorderStyle = FormBorderStyle.None;
            pdfWebView.Location = new Point(0, 0);
            pdfWebView.Size = WebViewVirtualPanel.ClientSize;

            WebViewVirtualPanel.Controls.Add(pdfWebView);
            if (!pdfWebView.Visible) pdfWebView.Show();

            WebViewIsDetached = false;
        }

        private void DetachWebView()
        {
            WebViewVirtualPanel.Controls.Remove(pdfWebView);
            pdfWebView.TopLevel = true;
            pdfWebView.FormBorderStyle = FormBorderStyle.Sizable;

            WebViewIsDetached = true;
        }

        private void PdfWebView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WebViewIsDetached)
            {
                e.Cancel = true;
                AttachWebView();
            }
        }

        // 주어진 PDF 파일을 앱 내에서 염
        // 직접 webview를 조작하여 open 시 잘못된 동작으로 PDF가 열리지 않음.
        // 무조건 이 메소드를 이용해서 열어야 함.
        private void OpenPDFInApp(string title, string path, int page)
        {
            noFileLabel.Visible = false;

            FilenameLabel.Text = title;

            pdfWebView.OpenPDFInApp(path, page);
        }

        private void DetachButton_Click(object sender, EventArgs e)
        {
            if (WebViewIsDetached) AttachWebView();
            else DetachWebView();
        }
        #endregion 웹뷰 관련

        #region 인덱서 관련
        private void FindAllPdfFiles(string path, bool recursive = false)
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                if (file.EndsWith(".pdf"))
                {
                    pdfs.Add(file);
                }
            }

            if (recursive)
            {
                var dirs = Directory.GetDirectories(path);
                foreach (var dir in dirs)
                {
                    FindAllPdfFiles(dir, true);
                }
            }
        }

        private async void IndexAll()
        {
            IsIndexing = true;

            QueryInputBox.Text = "";

            await Task.Run(() =>
            {
                pdfs.Clear();
                FindAllPdfFiles(basePath, true);

                ProgressPanelInUse = true;
                ProgressPanel.BeginInvoke((MethodInvoker)delegate
                {
                    ProgressPanel.Visible = true;
                });
                Indexer.OnIndexProgressUpdate += (progress) =>
                {
                    float percent = (float)Math.Round(progress * 100, 1);

                    IndexProgressTextLabel.BeginInvoke((MethodInvoker)delegate
                    {
                        IndexProgressTextLabel.Text = "인덱싱";
                    });

                    IndexProgressBar.BeginInvoke((MethodInvoker)delegate
                    {
                        IndexProgressBar.Style = ProgressBarStyle.Blocks;
                        IndexProgressBar.Value = (int)(percent * 10);
                    });

                    IndexProgressLabel.BeginInvoke((MethodInvoker)delegate
                    {
                        IndexProgressLabel.Text = $"{percent}%";
                    });
                };

                Indexer.IndexPdfs(pdfs.ToArray());

                ProgressPanelInUse = false;
                AutohideProgressPanel();

                new ToastContentBuilder()
                    .AddText("인덱싱 완료")
                    .AddText($"{pdfs.Count}개 문서가 인덱싱되었습니다.")
                    .Show();
            });

            IsIndexing = false;
        }
        #endregion 인덱서 관련

        #region 검색 관련 이벤트
        private void QueryInputBox_TextChanged(object sender, EventArgs e)
        {
            // 검색 쿼리 입력 시 자동 검색
            SearchResultPanel.Controls.Clear();

            var query = QueryInputBox.Text;
            var topDocs = Searcher.SearchQuery(query, 50);
            if (topDocs == null) return;

            var groups = Searcher.GroupDocuments(topDocs.ScoreDocs);
            foreach (var group in groups)
            {
                string relativePath = group.Path.Replace($"{basePath}\\", "").Replace($"\\{group.Title}.pdf", "");
                var searchItem = new SearchItemControl(group.Title, group.Path, relativePath, group.MatchPages, group, SearchResultPanel);
                searchItem.OnItemClick += SearchItem_OnItemClick;
                SearchResultPanel.Controls.Add(searchItem);
            }
        }

        private void SearchItem_OnItemClick(string title, string path, int page)
        {
            OpenPDFInApp(title, path, page);
        }
        #endregion 검색 관련 이벤트

        #region 인덱서 관련 이벤트
        private void IndexAllButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "정말로 모두 새로 인덱싱하겠습니까?\n\n주의: 이 작업은 오래걸릴 수도 있습니다.",
                "모두 새로 인덱싱",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes) return;

            IndexAll();
        }
        #endregion 인덱서 관련 이벤트

        private void OpenInNewWindowButton_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", pdfWebView.currentPdf.LocalPath.ToString());
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            //IsIndexing = true;

            //await Task.Run(() =>
            //{
            //    FindAllPdfFiles(basePath, true);
            //    var missing = Indexer.GetMissingPdfs(indexer, pdfs.ToArray());
            //    Debug.WriteLine("Missing:");
            //    foreach (var m in missing) Debug.WriteLine(m);
            //});

            //IsIndexing = false;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            IndexProgressTextLabel.Text = LuceneProvider.Ready ? "Ready" : "Not Ready";
            LuceneProvider.OnReady += () =>
            {
                if (Visible)
                {
                    IndexProgressTextLabel.BeginInvoke((MethodInvoker)delegate
                    {
                        IndexProgressTextLabel.Text = LuceneProvider.Ready ? "Ready" : "Not Ready";
                    });
                }
            };

            await Task.Run(() => Thread.Sleep(100));

            if (LuceneProvider.NotIndexedYet)
            {
                var result = MessageBox.Show("인덱싱이 없습니다.\n\n인덱싱을 지금 시작할까요?", "환영합니다", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    IndexAll();
                }
            }
        }

        private void DuplicateMangerButton_Click(object sender, EventArgs e)
        {
            duplicateManagerView.ShowDialog();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && AppSettings.MinimizeToTray)
            {
                HideMainUI();
            }

            if (!WebViewIsDetached)
            {
                pdfWebView.Size = WebViewVirtualPanel.ClientSize;
            }

            noFileLabel.Location = new Point(
                (WebViewVirtualPanel.ClientSize.Width / 2) - (noFileLabel.ClientSize.Width / 2),
                (WebViewVirtualPanel.ClientSize.Height / 2) - (noFileLabel.ClientSize.Height / 2)
            );
        }

        private void HideMainUI()
        {
            WindowState = FormWindowState.Minimized;
            Hide();

            if (!AppSettings.HintTrayIcon)
            {
                new ToastContentBuilder()
                    .AddText("PDFIndexer가 백그라운드에서 실행중입니다")
                    .AddText("작업표시줄의 트레이 아이콘에서 확인할 수 있습니다.")
                    .Show();

                AppSettings.HintTrayIcon = true;
                AppSettings.Save();
            }
        }

        private void ShowMainUIFromMinimize()
        {
            if (!Visible)
            {
                Show();
                WindowState = FormWindowState.Normal;
                Activate();
            } else
            {
                Activate();
            }
        }

        private void AutohideProgressPanel()
        {
            new Thread(() =>
            {
                Thread.Sleep(10 * 1000);
                if (Visible && !ProgressPanelInUse)
                {
                    ProgressPanel.BeginInvoke((MethodInvoker)delegate
                    {
                        ProgressPanel.Visible = false;
                    });
                }
            }).Start();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowMainUIFromMinimize();
        }

        private void ShowMainUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMainUIFromMinimize();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forceQuit = true;
            Close();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if ((e as MouseEventArgs).Button == MouseButtons.Left)
            {
                ShowMainUIFromMinimize();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceQuit && AppSettings.CloseToTray)
            {
                e.Cancel = true;
                HideMainUI();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (BackgroundMode) HideMainUI();
        }
    }
}
