using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3
{
    public struct DocumentGroup
    {
        public string Title;
        public string Path;
        public int Page;
        public int Matches;

        public DocumentGroup(string title, string path, int page)
        {
            Title = title;
            Path = path;
            Page = page;
            Matches = 1;
        }
    }
}
