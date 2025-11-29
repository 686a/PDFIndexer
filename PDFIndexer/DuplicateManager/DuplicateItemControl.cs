using PDFIndexer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFIndexer.DuplicateManager
{
    internal class DuplicateItemControl : Button
    {
        public SearchItem[] Items { get; set; }
        private new FlowLayoutPanel Parent { get; set; }

        private bool IsLoad;
        private Label mainTitleLabel;

        public DuplicateItemControl()
        {
            ApplyStyle();
        }

        public DuplicateItemControl(SearchItem[] items, FlowLayoutPanel parent)
        {
            ApplyStyle();
            ApplyParent(parent);

            Items = items;
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

        private void Parent_ClientSizeChanged(object sender, EventArgs e)
        {
            Width = Parent.ClientSize.Width;
        }

        protected override void InitLayout()
        {
            base.InitLayout();

            CreateItems();
        }

        private void CreateItems()
        {
            if (Items.Length == 0) return;

            mainTitleLabel = new Label();
            mainTitleLabel.Text = Items[0].Title;
            Controls.Add(mainTitleLabel);

            IsLoad = true;
        }
    }
}
