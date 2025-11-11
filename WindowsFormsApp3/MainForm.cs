using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            BackColor = Color.Black;
            TransparencyKey = Color.Black;

            pictureBox1.Image = DrawRoundedRectangle(24);
        }

        private Bitmap DrawRoundedRectangle(int radius)
        {
            // https://github.com/dotnet/winforms/blob/62ebdb4b0d5cc7e163b8dc9331dc196e576bf162/src/System.Drawing.Common/src/System/Drawing/Graphics.cs#L734C9-L737C6
            // https://github.com/dotnet/winforms/blob/62ebdb4b0d5cc7e163b8dc9331dc196e576bf162/src/System.Drawing.Common/src/System/Drawing/Drawing2D/GraphicsPath.cs#L669
            Bitmap bitmap = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                using (Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255, 255)))
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddArc(Width - radius, 0, radius, radius, -90f, 90f);
                        path.AddArc(Width - radius, Height - radius, radius, radius, 0f, 90f);
                        path.AddArc(0, Height - radius, radius, radius, 90f, 90f);
                        path.AddArc(0, 0, radius, radius, 180f, 90f);
                        path.CloseFigure();

                        g.FillPath(brush, path);
                    }
                }
            }

            return bitmap;
        }
    }
}
