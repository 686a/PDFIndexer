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

        private void DuplicateManagerView_Load(object sender, EventArgs e)
        {
            LoadDuplicates();
        }

        private async void LoadDuplicates()
        {
            // 중복 파일 로드
            await Task.Run(() =>
            {
                var foundHashes = Indexer.GetDuplicateFiles(indexer);

                //label3.BeginInvoke((MethodInvoker) delegate
                //{
                //    label3.Text = foundHashes.Length.ToString();
                //});

                foreach (var hash in foundHashes)
                {
                    var items = Indexer.GetFilesFromHash(indexer, hash);
                    flowLayoutPanel1.BeginInvoke((MethodInvoker)delegate
                    {
                        var label = new Label();
                        label.AutoSize = true;
                        string text = "";
                        foreach (var item in items)
                        {
                            text += $"{item.Title} - {item.Path}\n";
                        }
                        label.Text = text;
                        flowLayoutPanel1.Controls.Add(label);
                    });
                }
            });
        }
    }
}
