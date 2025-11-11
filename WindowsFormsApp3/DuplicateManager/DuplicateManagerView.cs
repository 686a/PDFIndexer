using Lucene.Net.Index;
using Lucene.Net.Search;
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
    public partial class DuplicateManagerView : Form
    {
        Indexer indexer;

        public DuplicateManagerView(Indexer indexer)
        {
            this.indexer = indexer;

            InitializeComponent();
        }

        private async void DuplicateManagerView_Load(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                var foundHashes = Indexer.GetDuplicateFiles(indexer);

                label3.BeginInvoke((MethodInvoker) delegate
                {
                    label3.Text = foundHashes.Length.ToString();
                });

                foreach (var hash in foundHashes)
                {
                    var items = Indexer.GetFilesFromHash(indexer, hash);
                    flowLayoutPanel1.BeginInvoke((MethodInvoker)delegate
                    {
                        var label = new Label();
                        label.Text = $"{items[0].Title}";
                        flowLayoutPanel1.Controls.Add(label);
                    });
                }
            });
            
        }
    }
}
