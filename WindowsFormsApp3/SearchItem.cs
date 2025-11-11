using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3
{
    public class SearchItem
    {
        public string Title { get; set; }
        public string AbsolutePath { get; set; }
        public string Path { get; set; }
        public int Page { get; set; }
        public string Hash { get; set; }
        public long LastModified { get; set; }

        public SearchItem(string title, string absolutePath, int page)
        {
            Title = title;
            AbsolutePath = absolutePath;
            Page = page;
        }

        public SearchItem(string title, string absolutePath, int page, string hash)
        {
            Title = title;
            AbsolutePath = absolutePath;
            Page = page;
            Hash = hash;
        }

        public SearchItem(string title, string absolutePath, string path, int page, string hash)
        {
            Title = title;
            AbsolutePath = absolutePath;
            Path = path;
            Page = page;
            Hash = hash;
        }

        public SearchItem(string title, string absolutePath, string path, int page, long lastModified)
        {
            Title = title;
            AbsolutePath = absolutePath;
            Path = path;
            Page = page;
            LastModified = lastModified;
        }

        public SearchItem(string title, string absolutePath, string path, int page, string hash, long lastModified)
        {
            Title = title;
            AbsolutePath = absolutePath;
            Path = path;
            Page = page;
            Hash = hash;
            LastModified = lastModified;
        }

        public SearchItem(Document doc)
        {
            Title = doc.Get("title");
            AbsolutePath = doc.Get("path");
            Page = int.Parse(doc.Get("page"));
            Hash = doc.Get("md5");
            LastModified = long.Parse(doc.Get("lastModified"));
        }
    }
}
