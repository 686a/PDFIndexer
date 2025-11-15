using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WindowsFormsApp3
{
    internal partial class SearchItemControl : Button
    {
        public string Title { get; set; }
        public string AbsolutePath { get; set; }
        public string Path { get; set; }
        public int Page { get; set; }
        public int Matches { get; set; }
        public new FlowLayoutPanel Parent { get; set; }

        private bool IsLoad = false;

        // Children
        private Label titleLabel;
        private Label pathLabel;
        private Label pageLabel;
        private Label matchesLabel;

        public SearchItemControl()
        {
            ApplyStyle();
        }

        public SearchItemControl(FlowLayoutPanel parent)
        {
            Height = 0;

            ApplyStyle();
            ApplyParent(parent);
        }

        // FlowLayoutPanel의 사이즈가 변경되면 컨트롤 사이즈도 같이 변경
        private void Parent_ClientSizeChanged(object sender, EventArgs e)
        {
            Width = Parent.ClientSize.Width;
        }

        public SearchItemControl(string title, string absolutePath, string path, int page, int matches, FlowLayoutPanel parent)
        {
            ApplyStyle();

            Title = title;
            AbsolutePath = absolutePath;
            Path = path;
            Page = page;
            Matches = matches;
            Parent = parent;

            ApplyParent(parent);
            AutoSize = true;

            //CreateItems();
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            CreateItems();
        }

        // 컨트롤 사이즈 변경 시 자식 컨트롤의 위치도 업데이트
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (IsLoad)
            {
                titleLabel.Location = new Point(8, Height / 2 - titleLabel.Height);
                pathLabel.Location = new Point(8, Height / 2);
                pageLabel.Location = new Point(titleLabel.Location.X + titleLabel.Width, titleLabel.Location.Y);
            }
        }

        private void ApplyStyle()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Margin = Padding.Empty;
            Padding = Padding.Empty;
        }

        private void ApplyParent(FlowLayoutPanel parent)
        {
            Parent = parent;
            Width = Parent.ClientSize.Width;

            Parent.ClientSizeChanged += Parent_ClientSizeChanged;
        }

        private void CreateItems()
        {
            titleLabel = new Label();
            titleLabel.Text = Title;
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font(
                "맑은 고딕",
                titleLabel.Font.Size,
                FontStyle.Bold,
                titleLabel.Font.Unit
            );
            titleLabel.BackColor = Color.Transparent;
            PassthroughEvents(titleLabel);
            Controls.Add(titleLabel);

            pathLabel = new Label();
            pathLabel.Text = Path;
            pathLabel.AutoSize = true;
            pathLabel.BackColor = Color.Transparent;
            PassthroughEvents(pathLabel);
            Controls.Add(pathLabel);

            pageLabel = new Label();
            pageLabel.Text = $"p{Page}";
            pageLabel.AutoSize = true;
            pageLabel.BackColor = Color.Transparent;
            PassthroughEvents(pageLabel);
            Controls.Add(pageLabel);

            IsLoad = true;
        }

        private void PassthroughEvents(Control control)
        {
            control.MouseHover += (_, e) => base.OnMouseHover(e);
            control.MouseEnter += (_, e) => base.OnMouseEnter(e);
            control.MouseLeave += (_, e) => base.OnMouseLeave(e);
            control.MouseMove += (_, e) => base.OnMouseMove(e);
            control.MouseClick += (s, e) => base.OnMouseClick(e);
            control.MouseDoubleClick += (s, e) => base.OnMouseDoubleClick(e);
            control.MouseDown += (s, e) => base.OnMouseDown(e);
            control.MouseUp += (s, e) => base.OnMouseUp(e);
            control.GotFocus += (s, e) => base.OnGotFocus(e);
            control.LostFocus += (s, e) => base.OnLostFocus(e);
        }

        public override string Text => $"\n\n\n\n";

        public override string ToString()
        {
            return $"{Title} - \n{Page} page";
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }
    }
}
