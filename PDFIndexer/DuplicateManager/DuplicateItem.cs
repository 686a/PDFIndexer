namespace PDFIndexer.DuplicateManager
{
    internal class DuplicateItem
    {
        public string Path { get; set; }
        public string MD5 { get; set; }
        public long LastModified { get; set; }

        public DuplicateItem(string path, string md5, long lastModified)
        {
            Path = path;
            MD5 = md5;
            LastModified = lastModified;
        }
    }
}
