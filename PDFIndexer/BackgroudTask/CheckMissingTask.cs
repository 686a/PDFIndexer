using PDFIndexer.BackgroundTask;
using PDFIndexer.Models.Database;
using PDFIndexer.SearchEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace PDFIndexer.BackgroudTask
{
    internal class CheckMissingTask : AbstractTask
    {
        private static readonly Properties.Settings AppSettings = Properties.Settings.Default;

        public override string Name => "새 파일 확인";
        public override string Description => null;

        public override void Run()
        {
            var files = new List<string>();
            Indexer.FindAllPdfFiles(ref files, AppSettings.BasePath, true);

            var missingAll = new List<string>();
            var missingOnlyOCR = new List<KeyValuePair<string, int>>(); // Pair<Path, Page>

            var dbCollection = DBContext.DB.GetCollection<IndexedDocument>("indexed");
            foreach (var file in files)
            {
                var dbItem = dbCollection.FindOne(LiteDB.Query.EQ("Path", file));
                if (dbItem == null)
                {
                    missingAll.Add(file);
                } else
                {
                    // OCR 미활성화
                    if (!AppSettings.OCREnabled) continue;

                    // 인덱스는 있는데, OCR이 되지 않은 페이지 찾기
                    var dbMissingOCRItems = dbCollection.Find(LiteDB.Query.And(
                        LiteDB.Query.EQ("Path", file),
                        LiteDB.Query.EQ("OCRDone", false)));

                    foreach (var item in dbMissingOCRItems)
                    {
                        missingOnlyOCR.Add(new KeyValuePair<string, int>(file, item.Page));
                    }
                }
            }

            // Enqueue index task
            foreach (string path in missingAll)
            {
                TaskManager.Enqueue(new IndexTask(path), priority: true);
            }

            // Enqueue OCR task
            foreach (KeyValuePair<string, int> missing in missingOnlyOCR)
            {
                TaskManager.Enqueue(new OCRTask(missing.Key, missing.Value));
            }
        }

        public override string GetTaskHash()
        {
            string id = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            return $"CheckMissingTask-{id}";
        }
    }
}
