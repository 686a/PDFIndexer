using LiteDB;
using System.IO;

namespace PDFIndexer
{
    internal class DBContext
    {
        private static readonly Properties.Settings AppSettings = Properties.Settings.Default;

        private static LiteDatabase _DB;
        public static LiteDatabase DB
        {
            get
            {
                return _DB;
            }
        }

        public DBContext()
        {
            _DB = new LiteDatabase(Path.Combine(AppSettings.BasePath, ".pdfindexer", "data.db"));
        }

        public static void Dispose()
        {
            _DB?.Dispose();
        }
    }
}
