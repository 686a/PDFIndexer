using PDFIndexer.BackgroudTask;
using PDFIndexer.BackgroundTask;
using PDFIndexer.DuplicateManager;
using PDFIndexer.Models.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFIndexer
{
    public partial class DuplicateManagerView : Form
    {
        private static readonly Properties.Settings AppSettings = Properties.Settings.Default;

        private Dictionary<string, long> SelectedFiles = new Dictionary<string, long>();

        public DuplicateManagerView()
        {
            InitializeComponent();
        }

        private void DuplicateManagerView_Load(object sender, EventArgs e)
        {
            LoadDuplicates();
        }

        private async void LoadDuplicates()
        {
            SelectedTotalSizeLabel.Text = "";

            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = 0;
            tableLayoutPanel1.RowStyles.Clear();

            SelectedFiles.Clear();

            var duplicates = new Dictionary<string, HashSet<DuplicateItem>>();
            var sizes = new Dictionary<string, long>();

            // 중복 파일 로드
            await Task.Run(() =>
            {
                var found = new List<string>();
                FindAllPdfFiles(ref found, AppSettings.BasePath, true);

                var dbCollection = DBContext.DB.GetCollection<IndexedDocument>("indexed");
                foreach (var path in found)
                {
                    var dbItem = dbCollection.FindOne(LiteDB.Query.EQ("Path", path));
                    if (dbItem == null) continue;

                    var record = new DuplicateItem(dbItem.Path, dbItem.MD5, dbItem.LastModified);
                    if (duplicates.ContainsKey(dbItem.MD5))
                    {
                        if (!duplicates[dbItem.MD5].Contains(record))
                            duplicates[dbItem.MD5].Add(record);
                    } else
                    {
                        duplicates.Add(dbItem.MD5, new HashSet<DuplicateItem> { record });
                        sizes.Add(dbItem.MD5, new FileInfo(path).Length);
                    }
                }
            });

            foreach (var dup in duplicates) {
                if (dup.Value.Count == 1) continue;

                // 파일 사이즈
                long size = sizes[dup.Key];
                float totalSize = (float)Math.Round(size * dup.Value.Count / 1024f);
                string totalSizeUnit = "KB";
                if (totalSize >= 1024)
                {
                    totalSize = (float)Math.Round(totalSize / 1024f, 2);
                    totalSizeUnit = "MB";
                }

                var list = new DuplicateItemListControl
                {
                    FileSize = size,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    CheckOnClick = true,
                    Margin = new Padding(0, 0, 0, 32),
                };

                // 헤더 테이블
                var header = new TableLayoutPanel()
                {
                    RowCount = 0,
                    ColumnCount = 0,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    AutoSize = true,
                };
                header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                //header.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

                // 라벨
                var label = new Label()
                {
                    Text = $"{dup.Value.Count}개 항목 ({totalSize}{totalSizeUnit})",
                    AutoSize = true,
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
                    TextAlign = ContentAlignment.MiddleLeft,
                };
                header.Controls.Add(label, 0, 0);

                // 리스트 버튼 - 모두 선택
                var selectAllButton = new Button()
                {
                    Text = "모두 선택",
                    AutoSize = true,
                };
                selectAllButton.Click += (s, e) =>
                {
                    for (int i = 0; i < list.Items.Count; i++)
                    {
                        list.SetItemChecked(i, true);
                    }
                };
                header.Controls.Add(selectAllButton, 1, 0);
                // 리스트 버튼 - 모두 선택 취소
                var selectNoneButton = new Button()
                {
                    Text = "모두 선택 취소",
                    AutoSize = true,
                };
                selectNoneButton.Click += (s, e) =>
                {
                    for (int i = 0; i < list.Items.Count; i++)
                    {
                        list.SetItemChecked(i, false);
                    }
                };
                header.Controls.Add(selectNoneButton, 2, 0);
                // 리스트 버튼 - 선택 반전
                var selectInvertButton = new Button()
                {
                    Text = "선택 반전",
                    AutoSize = true,
                };
                selectInvertButton.Click += (s, e) =>
                {
                    for (int i = 0; i < list.Items.Count; i++)
                    {
                        list.SetItemChecked(i, !list.GetItemChecked(i));
                    }
                };
                header.Controls.Add(selectInvertButton, 3, 0);

                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayoutPanel1.Controls.Add(header);

                foreach (var item in dup.Value)
                {
                    list.Items.Add(item.Path);
                }
                list.ItemCheck += List_ItemCheck;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tableLayoutPanel1.Controls.Add(list);
                list.ClientSize = new Size(1, list.GetItemHeight(0) * list.Items.Count);
            }
        }

        private void List_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            DuplicateItemListControl list = (DuplicateItemListControl)sender;
            string item = (string)list.Items[e.Index];
            if (e.NewValue == CheckState.Checked)
            {
                if (!SelectedFiles.ContainsKey(item))
                    SelectedFiles.Add(item, list.FileSize);
            }
            else
            {
                SelectedFiles.Remove(item);
            }

            UpdateSelectedTotalSize();
        }

        private void UpdateSelectedTotalSize()
        {
            long total = SelectedFiles.Values.Sum();
            float converted = (float)Math.Round(total / 1024f, 2);
            string convertedUnit = "KB";
            if (converted >= 1024)
            {
                converted = (float)Math.Round(total / 1024f / 1024f, 2);
                convertedUnit = "MB";
            } else if (converted >= 1024 * 1024)
            {
                converted = (float)Math.Round(total / 1024f / 1024f / 1024f, 2);
                convertedUnit = "GB";
            }

            SelectedTotalSizeLabel.Text = $"{SelectedFiles.Count}개 ({converted}{convertedUnit})";
        }

        private void FindAllPdfFiles(ref List<string> found, string path, bool recursive = false)
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                if (file.EndsWith(".pdf"))
                {
                    found.Add(file);
                }
            }

            if (recursive)
            {
                var dirs = Directory.GetDirectories(path);
                foreach (var dir in dirs)
                {
                    FindAllPdfFiles(ref found, dir, true);
                }
            }
        }

        private void ReloadButton_Click()
        {
            LoadDuplicates();
        }

        private async void SelectDeleteButton_Click()
        {
            var confirm = MessageBox.Show($"정말로 선택한 {SelectedFiles.Count}개의 파일을 삭제할까요?\n\n이 작업은 되둘릴 수 없습니다!", "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            var basePath = AppSettings.BasePath;
            var failed = new Dictionary<string, string>(); // 경로, 이유

            await Task.Run(() =>
            {
                foreach (var file in SelectedFiles)
                {
                    string path = file.Key;
                    try
                    {
                        File.Delete(path);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        // TODO: 이미 없음
                    } catch (UnauthorizedAccessException)
                    {
                        failed.Add(path.Replace(basePath, ""), "권한 없음");
                    } catch (IOException)
                    {
                        failed.Add(path.Replace(basePath, ""), "다른 프로그램이 파일 사용중");
                    } catch
                    {
                        failed.Add(path.Replace(basePath, ""), "알 수 없음");
                    }

                    TaskManager.Enqueue(new RemoveIndexTask(path), priority: true);
                }
            });

            if (failed.Count == 0)
            {
                MessageBox.Show($"모든 {SelectedFiles.Count}개 파일을 성공적으로 삭제했습니다.", "삭제 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
                var message = $"파일 {SelectedFiles.Count}개 중 {failed.Count}개 파일 삭제를 실패했습니다.\n\n";
                foreach (var fail in failed)
                {
                    message += $"- {fail.Value} : {fail.Key}\n";
                }
                MessageBox.Show(message, "삭제 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Reload
            LoadDuplicates();
        }

        private void SelectNonebutton_Click()
        {
            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                var item = tableLayoutPanel1.Controls[i];
                if (item is CheckedListBox list)
                {
                    for (int j = 0; j < list.Items.Count; j++)
                    {
                        list.SetItemChecked(j, false);
                    }
                }
            }
        }

        private void SelectAllButton_Click()
        {
            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                var item = tableLayoutPanel1.Controls[i];
                if (item is CheckedListBox list)
                {
                    for (int j = 0; j < list.Items.Count; j++)
                    {
                        list.SetItemChecked(j, true);
                    }
                }
            }
        }

        private void InvertSelectionButton_Click()
        {
            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                var item = tableLayoutPanel1.Controls[i];
                if (item is CheckedListBox list)
                {
                    for (int j = 0; j < list.Items.Count; j++)
                    {
                        list.SetItemChecked(j, !list.GetItemChecked(j));
                    }
                }
            }
        }
    }
}
