using Lucene.Net.Index;
using Lucene.Net.Search;
using PDFIndexer.SearchEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFIndexer
{
    public partial class DuplicateManagerView : Form
    {
        public DuplicateManagerView()
        {
            InitializeComponent();
        }

        private void DuplicateManagerView_Load(object sender, EventArgs e)
        {
            LoadDuplicates();
        }

        private async void LoadDuplicates()
        {
            // 중복 파일 로드
            await Task.Run(() =>
            {
                // TODO:
            });
        }
    }
}
