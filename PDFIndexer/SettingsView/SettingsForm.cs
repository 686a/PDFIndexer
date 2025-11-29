using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFIndexer.SettingsView
{
    public partial class SettingsForm : Form
    {
#if DEBUG
        private static string Build = "Debug";
#else
        private static string Build = "Release";
#endif

        private Properties.Settings AppSettings = Properties.Settings.Default;

        private bool SettingsLoaded = false;

        public SettingsForm()
        {
            InitializeComponent();

            Size = MinimumSize;

            LoadSettings();

            VersionLabel.Text = VersionLabel.Text
                .Replace("{VERSION}", Application.ProductVersion.ToString())
                .Replace("{BUILD}", Build);
        }

        public void LoadSettings()
        {
            SettingsLoaded = false;

            BasePathTextBox.Text = AppSettings.BasePath;

            OCREnabledCheckBox.Checked = AppSettings.OCREnabled;
            OCREngineComboBox.SelectedIndex = AppSettings.OCREngine;
            OCRCPUPriorityComboBox.SelectedIndex = AppSettings.OCRCPUPriority;

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (key != null)
                {
                    var value = key.GetValue("PDFIndexer");
                    AutorunCheckBox.Checked = value != null;
                }
                else AutorunCheckBox.Checked = false;
            } catch
            {
                AutorunCheckBox.Checked = false;
            }
            CloseToTrayCheckBox.Checked = AppSettings.CloseToTray;
            MinimizeToTrayCheckBox.Checked = AppSettings.MinimizeToTray;

            SettingsLoaded = true;
        }

        private void RestartConfirm()
        {
            var result = MessageBox.Show(
                "이 설정은 다음 시작 시 적용됩니다.\n\n지금 다시시작할까요?",
                "확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        // 기준 경로 선택
        private void SelectBaseFolderButton_Click(object sender, EventArgs e)
        {
            DialogResult result = SelectFolderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var dir = SelectFolderDialog.SelectedPath;

                // 동일 경로 선택 시 무시
                if (dir == AppSettings.BasePath)
                {
                    return;
                }

                var confirm = MessageBox.Show(
                    "정말로 기준 경로를 바꾸시겠습니까?\n\n기준 경로를 변경하려면, 프로그램을 재시작해야합니다.",
                    "기준 경로 변경",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.OK)
                {
                    BasePathTextBox.Text = dir;
                    AppSettings.BasePath = dir;
                    AppSettings.Save();

                    Application.Restart();
                }
            }
        }

        private void OCREnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!SettingsLoaded) return;

            AppSettings.OCREnabled = OCREnabledCheckBox.Checked;
            AppSettings.Save();

            RestartConfirm();
        }

        private void OCRCPUPriorityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SettingsLoaded) return;

            AppSettings.OCRCPUPriority = OCRCPUPriorityComboBox.SelectedIndex;
            AppSettings.Save();

            RestartConfirm();
        }

        private void AutorunCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!SettingsLoaded) return;

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Assembly.GetExecutingAssembly().Location,
                Arguments = "--install-startup",
                Verb = "runas",
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            if (!AutorunCheckBox.Checked)
            {
                startInfo.Arguments = "--uninstall-startup";
            }

            try
            {
                var process = Process.Start(startInfo);
                process.WaitForExit();
            }
            catch (Win32Exception) { }

            LoadSettings();
        }

        private void CloseToTrayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!SettingsLoaded) return;

            AppSettings.CloseToTray = CloseToTrayCheckBox.Checked;
            AppSettings.Save();
        }

        private void MinimizeToTrayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!SettingsLoaded) return;

            AppSettings.MinimizeToTray = MinimizeToTrayCheckBox.Checked;
            AppSettings.Save();
        }

        private void ResetSettingsButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "정말로 설정을 초기화할까요?\n\n이 작업은 되돌릴 수 없습니다",
                "설정 초기화",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                AppSettings.Reset();
                Application.Restart();
            }
        }
    }
}
