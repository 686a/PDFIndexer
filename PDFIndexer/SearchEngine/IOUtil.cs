using System;
using System.IO;
using System.Security.Cryptography;

namespace PDFIndexer.SearchEngine
{
    internal class IOUtil
    {
        public static string GetHashFromFile(string path)
        {
            // https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.md5?view=net-9.0
            var md5 = MD5.Create();
            using (var stream = File.OpenRead(path))
            {
                var hash = md5.ComputeHash(stream);
                var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();

                return hashString;
            }
        }

        public static long GetLastModifiedFromFile(string path)
        {
            return ((DateTimeOffset)File.GetLastWriteTimeUtc(path)).ToUnixTimeSeconds();
        }
    }
}
