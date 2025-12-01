using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFIndexer.SetupWizard.Phases
{
    internal class OCRPhase : Phase
    {
        private Properties.Settings AppSettings = Properties.Settings.Default;
        private SetupWizardForm MainUI;

        private Label TitleLabel;
        private Label DescriptionLabel;
        private CheckBox UseOCRCheckBox;

        public OCRPhase(SetupWizardForm mainUI)
        {
            MainUI = mainUI;
        }

        public override void Setup()
        {
            TableLayoutPanel layout = MainUI.GetMainTableLayout();
            MainUI.ClearMainTable();

            Initialize();

            layout.ColumnCount = 1;
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            layout.Controls.Add(TitleLabel, 0, 0);
            layout.Controls.Add(DescriptionLabel, 0, 1);
            layout.Controls.Add(UseOCRCheckBox, 0, 2);
        }

        protected override void Initialize()
        {
            if (Initialized) return;

            TitleLabel = new Label()
            {
                Text = "OCR 기능을 사용할까요?",
                Font = new System.Drawing.Font("Malgun Gothic", 12F),
                AutoSize = true,
            };

            DescriptionLabel = new Label()
            {
                Text = "\nOCR 기능은 :\n" +
                "- 이미지 내 텍스트를 검색할 수 있도록 합니다.\n" +
                "- 백그라운드에서 수행하고, 기본 기능과 별개로 동작합니다.\n" +
                "- CPU, 메모리 사용량이 다소 높습니다.\n" +
                "- x64, AVX2를 지원하는 CPU에서만 사용가능합니다.\n" +
                "- 나중에 설정에서 자세한 설정과 켜거나 끌 수 있습니다.\n\n",
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                AutoSize = true,
            };

            UseOCRCheckBox = new CheckBox()
            {
                Text = "&OCR 기능 활성화",
                Checked = true,
                AutoSize = true,
            };

            Initialized = true;
        }

        public override void NextButton_Click(object sender, EventArgs e)
        {
            AppSettings.OCREnabled = UseOCRCheckBox.Checked;
            AppSettings.Save();

            MainUI.NextPhase();
        }

        public override void PrevButton_Click(object sender, EventArgs e)
        {
            MainUI.PrevPhase();
        }
    }
}
