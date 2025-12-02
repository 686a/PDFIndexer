namespace PDFIndexer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayIconMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowMainUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.QueryInputBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.SearchResultPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.FilenameLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.DetachButton = new System.Windows.Forms.Button();
            this.OpenInNewWindowButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.WebViewVirtualPanel = new System.Windows.Forms.Panel();
            this.noFileLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.IndexAllButton = new PDFIndexer.IconTextButtonControl();
            this.IndexMissingButton = new PDFIndexer.IconTextButtonControl();
            this.DuplicateManagerButton = new PDFIndexer.IconTextButtonControl();
            this.ProgressPanel = new System.Windows.Forms.TableLayoutPanel();
            this.IndexProgressPanel = new System.Windows.Forms.TableLayoutPanel();
            this.IndexProgressLabel = new System.Windows.Forms.Label();
            this.IndexProgressBar = new System.Windows.Forms.ProgressBar();
            this.IndexProgressTextLabel = new System.Windows.Forms.Label();
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.BackgroundTaskStatusTextLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.BackgroundTaskStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TrayIconMenuStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.WebViewVirtualPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.ProgressPanel.SuspendLayout();
            this.IndexProgressPanel.SuspendLayout();
            this.MainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // TrayIcon
            // 
            this.TrayIcon.ContextMenuStrip = this.TrayIconMenuStrip;
            this.TrayIcon.Icon = global::PDFIndexer.Properties.Resources.PDFIndexerIcon_Tiny;
            this.TrayIcon.Text = "PDFIndexer";
            this.TrayIcon.Visible = true;
            this.TrayIcon.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.TrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // TrayIconMenuStrip
            // 
            this.TrayIconMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowMainUIToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.TrayIconMenuStrip.Name = "contextMenuStrip1";
            this.TrayIconMenuStrip.Size = new System.Drawing.Size(142, 48);
            // 
            // ShowMainUIToolStripMenuItem
            // 
            this.ShowMainUIToolStripMenuItem.Name = "ShowMainUIToolStripMenuItem";
            this.ShowMainUIToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.ShowMainUIToolStripMenuItem.Text = "나타내기 (&S)";
            this.ShowMainUIToolStripMenuItem.Click += new System.EventHandler(this.ShowMainUIToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.ExitToolStripMenuItem.Text = "종료 (&E)";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1225, 744);
            this.tableLayoutPanel1.TabIndex = 25;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.QueryInputBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1219, 28);
            this.tableLayoutPanel3.TabIndex = 27;
            // 
            // QueryInputBox
            // 
            this.QueryInputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QueryInputBox.Font = new System.Drawing.Font("Malgun Gothic", 10F);
            this.QueryInputBox.Location = new System.Drawing.Point(28, 4);
            this.QueryInputBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.QueryInputBox.Name = "QueryInputBox";
            this.QueryInputBox.Size = new System.Drawing.Size(1188, 25);
            this.QueryInputBox.TabIndex = 26;
            this.QueryInputBox.TextChanged += new System.EventHandler(this.QueryInputBox_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PDFIndexer.Properties.Resources.SearchIcon;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(19, 21);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.SearchResultPanel, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 37);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1219, 656);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // SearchResultPanel
            // 
            this.SearchResultPanel.AutoScroll = true;
            this.SearchResultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchResultPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.SearchResultPanel.Location = new System.Drawing.Point(3, 52);
            this.SearchResultPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SearchResultPanel.Name = "SearchResultPanel";
            this.SearchResultPanel.Size = new System.Drawing.Size(343, 600);
            this.SearchResultPanel.TabIndex = 17;
            this.SearchResultPanel.WrapContents = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 48);
            this.label3.TabIndex = 15;
            this.label3.Text = "검색 결과";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.FilenameLabel, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.flowLayoutPanel2, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(352, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(864, 42);
            this.tableLayoutPanel5.TabIndex = 16;
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.FilenameLabel.AutoSize = true;
            this.FilenameLabel.Location = new System.Drawing.Point(3, 0);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(53, 42);
            this.FilenameLabel.TabIndex = 7;
            this.FilenameLabel.Text = "";
            this.FilenameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.DetachButton);
            this.flowLayoutPanel2.Controls.Add(this.OpenInNewWindowButton);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(62, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(799, 36);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // DetachButton
            // 
            this.DetachButton.Image = global::PDFIndexer.Properties.Resources.OpenInNewIcon;
            this.DetachButton.Location = new System.Drawing.Point(767, 3);
            this.DetachButton.Name = "DetachButton";
            this.DetachButton.Size = new System.Drawing.Size(29, 29);
            this.DetachButton.TabIndex = 20;
            this.DetachButton.UseVisualStyleBackColor = true;
            this.DetachButton.Click += new System.EventHandler(this.DetachButton_Click);
            // 
            // OpenInNewWindowButton
            // 
            this.OpenInNewWindowButton.Image = global::PDFIndexer.Properties.Resources.OpenInBrowserIcon;
            this.OpenInNewWindowButton.Location = new System.Drawing.Point(732, 3);
            this.OpenInNewWindowButton.Name = "OpenInNewWindowButton";
            this.OpenInNewWindowButton.Size = new System.Drawing.Size(29, 29);
            this.OpenInNewWindowButton.TabIndex = 21;
            this.OpenInNewWindowButton.UseVisualStyleBackColor = true;
            this.OpenInNewWindowButton.Click += new System.EventHandler(this.OpenInNewWindowButton_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.WebViewVirtualPanel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(352, 51);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(864, 602);
            this.panel2.TabIndex = 18;
            // 
            // WebViewVirtualPanel
            // 
            this.WebViewVirtualPanel.BackColor = System.Drawing.Color.Transparent;
            this.WebViewVirtualPanel.Controls.Add(this.noFileLabel);
            this.WebViewVirtualPanel.Controls.Add(this.label2);
            this.WebViewVirtualPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebViewVirtualPanel.Location = new System.Drawing.Point(0, 0);
            this.WebViewVirtualPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.WebViewVirtualPanel.Name = "WebViewVirtualPanel";
            this.WebViewVirtualPanel.Size = new System.Drawing.Size(864, 602);
            this.WebViewVirtualPanel.TabIndex = 26;
            // 
            // noFileLabel
            // 
            this.noFileLabel.AutoSize = true;
            this.noFileLabel.Location = new System.Drawing.Point(365, 300);
            this.noFileLabel.Name = "noFileLabel";
            this.noFileLabel.Size = new System.Drawing.Size(135, 15);
            this.noFileLabel.TabIndex = 25;
            this.noFileLabel.Text = "선택된 파일이 없습니다";
            this.noFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "WebView Proxy\r\n이 부분에 PdfWebView가 표시됩니다";
            this.label2.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 349F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ProgressPanel, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 699);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1219, 42);
            this.tableLayoutPanel2.TabIndex = 28;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel3.Controls.Add(this.SettingsButton);
            this.flowLayoutPanel3.Controls.Add(this.IndexAllButton);
            this.flowLayoutPanel3.Controls.Add(this.IndexMissingButton);
            this.flowLayoutPanel3.Controls.Add(this.DuplicateManagerButton);
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(352, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(864, 36);
            this.flowLayoutPanel3.TabIndex = 26;
            // 
            // SettingsButton
            // 
            this.SettingsButton.Image = global::PDFIndexer.Properties.Resources.SettingsIcon;
            this.SettingsButton.Location = new System.Drawing.Point(831, 4);
            this.SettingsButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(30, 30);
            this.SettingsButton.TabIndex = 21;
            this.SettingsButton.UseVisualStyleBackColor = true;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // IndexAllButton
            // 
            this.IndexAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.IndexAllButton.Content = "모두 인덱싱";
            this.IndexAllButton.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.IndexAllButton.Icon = global::PDFIndexer.Properties.Resources.DatabaseIcon;
            this.IndexAllButton.IconSizeOffset = 0;
            this.IndexAllButton.Location = new System.Drawing.Point(711, 4);
            this.IndexAllButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IndexAllButton.Name = "IndexAllButton";
            this.IndexAllButton.Size = new System.Drawing.Size(114, 30);
            this.IndexAllButton.TabIndex = 25;
            this.IndexAllButton.UseVisualStyleBackColor = true;
            this.IndexAllButton.OnButtonClick += new PDFIndexer.IconTextButtonControl.ButtonClick(this.IndexAllButton_Click);
            // 
            // IndexMissingButton
            // 
            this.IndexMissingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.IndexMissingButton.Content = "새 파일 인덱싱";
            this.IndexMissingButton.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.IndexMissingButton.Icon = global::PDFIndexer.Properties.Resources.DatabaseUploadIcon;
            this.IndexMissingButton.IconSizeOffset = 0;
            this.IndexMissingButton.Location = new System.Drawing.Point(578, 4);
            this.IndexMissingButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IndexMissingButton.Name = "IndexMissingButton";
            this.IndexMissingButton.Size = new System.Drawing.Size(127, 30);
            this.IndexMissingButton.TabIndex = 26;
            this.IndexMissingButton.UseVisualStyleBackColor = true;
            this.IndexMissingButton.OnButtonClick += new PDFIndexer.IconTextButtonControl.ButtonClick(this.IndexMissingButton_OnButtonClick);
            // 
            // DuplicateManagerButton
            // 
            this.DuplicateManagerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.DuplicateManagerButton.Content = "중복 관리자";
            this.DuplicateManagerButton.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.DuplicateManagerButton.Icon = global::PDFIndexer.Properties.Resources.ContentCopyIcon;
            this.DuplicateManagerButton.IconSizeOffset = 0;
            this.DuplicateManagerButton.Location = new System.Drawing.Point(458, 4);
            this.DuplicateManagerButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DuplicateManagerButton.Name = "DuplicateManagerButton";
            this.DuplicateManagerButton.Size = new System.Drawing.Size(114, 30);
            this.DuplicateManagerButton.TabIndex = 24;
            this.DuplicateManagerButton.UseVisualStyleBackColor = true;
            this.DuplicateManagerButton.OnButtonClick += new PDFIndexer.IconTextButtonControl.ButtonClick(this.DuplicateMangerButton_Click);
            // 
            // ProgressPanel
            // 
            this.ProgressPanel.ColumnCount = 1;
            this.ProgressPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ProgressPanel.Controls.Add(this.IndexProgressPanel, 0, 0);
            this.ProgressPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgressPanel.Location = new System.Drawing.Point(3, 3);
            this.ProgressPanel.MaximumSize = new System.Drawing.Size(343, 36);
            this.ProgressPanel.Name = "ProgressPanel";
            this.ProgressPanel.RowCount = 1;
            this.ProgressPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ProgressPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ProgressPanel.Size = new System.Drawing.Size(343, 36);
            this.ProgressPanel.TabIndex = 27;
            this.ProgressPanel.Visible = false;
            // 
            // IndexProgressPanel
            // 
            this.IndexProgressPanel.AutoSize = true;
            this.IndexProgressPanel.ColumnCount = 3;
            this.IndexProgressPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.IndexProgressPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.IndexProgressPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.IndexProgressPanel.Controls.Add(this.IndexProgressLabel, 2, 0);
            this.IndexProgressPanel.Controls.Add(this.IndexProgressBar, 1, 0);
            this.IndexProgressPanel.Controls.Add(this.IndexProgressTextLabel, 0, 0);
            this.IndexProgressPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IndexProgressPanel.Location = new System.Drawing.Point(0, 0);
            this.IndexProgressPanel.Margin = new System.Windows.Forms.Padding(0);
            this.IndexProgressPanel.MaximumSize = new System.Drawing.Size(343, 36);
            this.IndexProgressPanel.Name = "IndexProgressPanel";
            this.IndexProgressPanel.RowCount = 1;
            this.IndexProgressPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.IndexProgressPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.IndexProgressPanel.Size = new System.Drawing.Size(343, 36);
            this.IndexProgressPanel.TabIndex = 28;
            // 
            // IndexProgressLabel
            // 
            this.IndexProgressLabel.AutoSize = true;
            this.IndexProgressLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IndexProgressLabel.Location = new System.Drawing.Point(292, 0);
            this.IndexProgressLabel.MinimumSize = new System.Drawing.Size(48, 1);
            this.IndexProgressLabel.Name = "IndexProgressLabel";
            this.IndexProgressLabel.Size = new System.Drawing.Size(48, 36);
            this.IndexProgressLabel.TabIndex = 3;
            this.IndexProgressLabel.Text = "0.0%";
            this.IndexProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // IndexProgressBar
            // 
            this.IndexProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.IndexProgressBar.Location = new System.Drawing.Point(49, 9);
            this.IndexProgressBar.Margin = new System.Windows.Forms.Padding(0);
            this.IndexProgressBar.Maximum = 1000;
            this.IndexProgressBar.MaximumSize = new System.Drawing.Size(0, 18);
            this.IndexProgressBar.Name = "IndexProgressBar";
            this.IndexProgressBar.Size = new System.Drawing.Size(240, 18);
            this.IndexProgressBar.TabIndex = 2;
            // 
            // IndexProgressTextLabel
            // 
            this.IndexProgressTextLabel.AutoSize = true;
            this.IndexProgressTextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IndexProgressTextLabel.Location = new System.Drawing.Point(3, 0);
            this.IndexProgressTextLabel.MinimumSize = new System.Drawing.Size(43, 1);
            this.IndexProgressTextLabel.Name = "IndexProgressTextLabel";
            this.IndexProgressTextLabel.Size = new System.Drawing.Size(43, 36);
            this.IndexProgressTextLabel.TabIndex = 1;
            this.IndexProgressTextLabel.Text = "인덱싱";
            this.IndexProgressTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainStatusStrip
            // 
            this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BackgroundTaskStatusTextLabel,
            this.BackgroundTaskStatusLabel});
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 759);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.Size = new System.Drawing.Size(1249, 22);
            this.MainStatusStrip.SizingGrip = false;
            this.MainStatusStrip.TabIndex = 26;
            // 
            // BackgroundTaskStatusTextLabel
            // 
            this.BackgroundTaskStatusTextLabel.Name = "BackgroundTaskStatusTextLabel";
            this.BackgroundTaskStatusTextLabel.Size = new System.Drawing.Size(31, 17);
            this.BackgroundTaskStatusTextLabel.Text = "준비";
            // 
            // BackgroundTaskStatusLabel
            // 
            this.BackgroundTaskStatusLabel.Name = "BackgroundTaskStatusLabel";
            this.BackgroundTaskStatusLabel.Size = new System.Drawing.Size(31, 17);
            this.BackgroundTaskStatusLabel.Text = "상태";
            this.BackgroundTaskStatusLabel.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1249, 781);
            this.Controls.Add(this.MainStatusStrip);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = global::PDFIndexer.Properties.Resources.PDFIndexerIcon;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(810, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDFIndexer";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.TrayIconMenuStrip.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.WebViewVirtualPanel.ResumeLayout(false);
            this.WebViewVirtualPanel.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.ProgressPanel.ResumeLayout(false);
            this.ProgressPanel.PerformLayout();
            this.IndexProgressPanel.ResumeLayout(false);
            this.IndexProgressPanel.PerformLayout();
            this.MainStatusStrip.ResumeLayout(false);
            this.MainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenuStrip TrayIconMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ShowMainUIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label FilenameLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button DetachButton;
        private System.Windows.Forms.Button OpenInNewWindowButton;
        private System.Windows.Forms.FlowLayoutPanel SearchResultPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox QueryInputBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button SettingsButton;
        private IconTextButtonControl DuplicateManagerButton;
        private System.Windows.Forms.Panel WebViewVirtualPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label noFileLabel;
        private IconTextButtonControl IndexAllButton;
        private IconTextButtonControl IndexMissingButton;
        private System.Windows.Forms.TableLayoutPanel ProgressPanel;
        private System.Windows.Forms.TableLayoutPanel IndexProgressPanel;
        private System.Windows.Forms.Label IndexProgressLabel;
        private System.Windows.Forms.ProgressBar IndexProgressBar;
        private System.Windows.Forms.Label IndexProgressTextLabel;
        private System.Windows.Forms.StatusStrip MainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel BackgroundTaskStatusTextLabel;
        private System.Windows.Forms.ToolStripStatusLabel BackgroundTaskStatusLabel;
    }
}

