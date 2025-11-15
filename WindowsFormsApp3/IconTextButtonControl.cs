using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class IconTextButtonControl : Button
    {
        public Image Icon { get; set; }
        public string Content { get; set; }

        private int _IconSizeOffset;

        [DefaultValue(8)]
        public int IconSizeOffset
        {
            get { return _IconSizeOffset; }
            set
            {
                _IconSizeOffset = value;
            }
        }

        private bool IsLoad = false;

        // Children
        private PictureBox pictureBox;
        private Label label;

        public IconTextButtonControl()
        {
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            CreateItems();
        }

        protected override void OnParentFontChanged(EventArgs e)
        {
            base.OnParentFontChanged(e);
        }

        private void CreateItems()
        {
            // Icon
            pictureBox = new PictureBox
            {
                Image = Icon,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
            };

            PassthroughEvents(pictureBox);
            Controls.Add(pictureBox);

            // Label
            label = new Label
            {
                Text = Content,
                BackColor = Color.Transparent,
                AutoSize = true,
            };

            PassthroughEvents(label);
            Controls.Add(label);

            IsLoad = true;
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            var iconSize = Math.Min(ClientSize.Width, ClientSize.Height) - 8;
            pictureBox.Size = new Size(iconSize, iconSize);

            // Get Boundary
            Size bound = new Size(0, 0);
            bound.Width = pictureBox.Width;
            bound.Height = pictureBox.Height;

            bound.Width += label.Width;
            bound.Height = Math.Max(pictureBox.Height, label.Height);

            // Set location from boundary
            Point center = new Point(Size.Width / 2, Size.Height / 2);

            Rectangle rect = new Rectangle(0, 0, 0, 0);
            rect.X = center.X - bound.Width / 2;
            rect.Y = center.Y - bound.Height / 2;
            rect.Width = bound.Width;
            rect.Height = bound.Height;

            pictureBox.Location = new Point(rect.X, rect.Y);
            label.Location = new Point(rect.X + pictureBox.Width, center.Y - label.Height / 2);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (IsLoad)
            {
                UpdateLayout();
            }
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
    }
}
