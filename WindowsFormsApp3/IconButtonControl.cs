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
    public partial class IconButtonControl : Button
    {
        public Image Icon { get; set; }

        public new string Text { get; }

        // Children
        private PictureBox pictureBox;

        public IconButtonControl()
        {
        }

        protected override void InitLayout()
        {
            base.InitLayout();
            CreateItems();
        }

        private void CreateItems()
        {
            pictureBox = new PictureBox
            {
                Image = Icon,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.CenterImage,
            };

            pictureBox.MouseHover += (_, e) => base.OnMouseHover(e);
            pictureBox.MouseEnter += (_, e) => base.OnMouseEnter(e);
            pictureBox.MouseLeave += (_, e) => base.OnMouseLeave(e);
            pictureBox.MouseMove += (_, e) => base.OnMouseMove(e);
            pictureBox.MouseClick += (s, e) => base.OnMouseClick(e);
            pictureBox.MouseDoubleClick += (s, e) => base.OnMouseDoubleClick(e);
            pictureBox.MouseDown += (s, e) => base.OnMouseDown(e);
            pictureBox.MouseUp += (s, e) => base.OnMouseUp(e);
            pictureBox.GotFocus += (s, e) => base.OnGotFocus(e);
            pictureBox.LostFocus += (s, e) => base.OnLostFocus(e);

            Controls.Add(pictureBox);
        }
    }
}
