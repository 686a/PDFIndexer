namespace PDFIndexer
{
    partial class DuplicateManagerView
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SelectedTotalSizeLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ReloadButton = new PDFIndexer.IconTextButtonControl();
            this.SelectDeleteButton = new PDFIndexer.IconTextButtonControl();
            this.InvertSelectionButton = new PDFIndexer.IconTextButtonControl();
            this.SelectNonebutton = new PDFIndexer.IconTextButtonControl();
            this.SelectAllButton = new PDFIndexer.IconTextButtonControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "중복 파일";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(343, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "중복 파일을 삭제하고 원본을 보관하여 저장공간을 확보하세요";
            // 
            // SelectedTotalSizeLabel
            // 
            this.SelectedTotalSizeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedTotalSizeLabel.Location = new System.Drawing.Point(791, 30);
            this.SelectedTotalSizeLabel.Name = "SelectedTotalSizeLabel";
            this.SelectedTotalSizeLabel.Size = new System.Drawing.Size(188, 15);
            this.SelectedTotalSizeLabel.TabIndex = 3;
            this.SelectedTotalSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 61);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(963, 439);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // ReloadButton
            // 
            this.ReloadButton.Content = "새로고침";
            this.ReloadButton.Icon = global::PDFIndexer.Properties.Resources.RefreshIcon;
            this.ReloadButton.IconSizeOffset = 0;
            this.ReloadButton.Location = new System.Drawing.Point(16, 506);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(95, 29);
            this.ReloadButton.TabIndex = 2;
            this.ReloadButton.UseVisualStyleBackColor = true;
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // SelectDeleteButton
            // 
            this.SelectDeleteButton.Content = "선택 삭제";
            this.SelectDeleteButton.Icon = global::PDFIndexer.Properties.Resources.DeleteIcon;
            this.SelectDeleteButton.IconSizeOffset = 0;
            this.SelectDeleteButton.Location = new System.Drawing.Point(880, 506);
            this.SelectDeleteButton.Name = "SelectDeleteButton";
            this.SelectDeleteButton.Size = new System.Drawing.Size(99, 29);
            this.SelectDeleteButton.TabIndex = 1;
            this.SelectDeleteButton.UseVisualStyleBackColor = true;
            this.SelectDeleteButton.Click += new System.EventHandler(this.SelectDeleteButton_Click);
            // 
            // InvertSelectionButton
            // 
            this.InvertSelectionButton.Content = "선택 반전";
            this.InvertSelectionButton.Icon = global::PDFIndexer.Properties.Resources.MoveSelectionRightIcon;
            this.InvertSelectionButton.IconSizeOffset = 0;
            this.InvertSelectionButton.Location = new System.Drawing.Point(763, 506);
            this.InvertSelectionButton.Name = "InvertSelectionButton";
            this.InvertSelectionButton.Size = new System.Drawing.Size(93, 29);
            this.InvertSelectionButton.TabIndex = 4;
            this.InvertSelectionButton.UseVisualStyleBackColor = true;
            this.InvertSelectionButton.Click += new System.EventHandler(this.InvertSelectionButton_Click);
            // 
            // SelectNonebutton
            // 
            this.SelectNonebutton.Content = "모두 선택 취소";
            this.SelectNonebutton.Icon = global::PDFIndexer.Properties.Resources.DeselectIcon;
            this.SelectNonebutton.IconSizeOffset = 0;
            this.SelectNonebutton.Location = new System.Drawing.Point(634, 506);
            this.SelectNonebutton.Name = "SelectNonebutton";
            this.SelectNonebutton.Size = new System.Drawing.Size(123, 29);
            this.SelectNonebutton.TabIndex = 3;
            this.SelectNonebutton.UseVisualStyleBackColor = true;
            this.SelectNonebutton.Click += new System.EventHandler(this.SelectNonebutton_Click);
            // 
            // SelectAllButton
            // 
            this.SelectAllButton.Content = "모두 선택";
            this.SelectAllButton.Icon = global::PDFIndexer.Properties.Resources.SelectAllIcon;
            this.SelectAllButton.IconSizeOffset = 0;
            this.SelectAllButton.Location = new System.Drawing.Point(531, 506);
            this.SelectAllButton.Name = "SelectAllButton";
            this.SelectAllButton.Size = new System.Drawing.Size(97, 29);
            this.SelectAllButton.TabIndex = 3;
            this.SelectAllButton.UseVisualStyleBackColor = true;
            this.SelectAllButton.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // DuplicateManagerView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(991, 547);
            this.Controls.Add(this.ReloadButton);
            this.Controls.Add(this.SelectDeleteButton);
            this.Controls.Add(this.InvertSelectionButton);
            this.Controls.Add(this.SelectNonebutton);
            this.Controls.Add(this.SelectAllButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.SelectedTotalSizeLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::PDFIndexer.Properties.Resources.PDFIndexerIcon;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "DuplicateManagerView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Duplicate Manager";
            this.Load += new System.EventHandler(this.DuplicateManagerView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label SelectedTotalSizeLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private IconTextButtonControl SelectAllButton;
        private IconTextButtonControl SelectNonebutton;
        private IconTextButtonControl InvertSelectionButton;
        private IconTextButtonControl SelectDeleteButton;
        private IconTextButtonControl ReloadButton;
    }
}