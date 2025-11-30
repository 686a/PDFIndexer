using System.Windows.Forms;

namespace PDFIndexer.DuplicateManager
{
    internal class DuplicateItemListControl : CheckedListBox
    {
        public long FileSize { get; set; }
    }
}
