using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFIndexer.Models.Database
{
    internal class IndexedDocument
    {
        public string _id { get; set; }
        public string Path { get; set; }
        public int Page { get; set; }
        public string MD5 { get; set; }
        public long LastModified { get; set; }
        public bool OCRDone { get; set; }
    }
}
