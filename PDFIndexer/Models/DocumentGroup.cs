using Lucene.Net.Search;
using System.Collections.Generic;

namespace PDFIndexer.Models
{
    public class DocumentGroup
    {
        public string _Title;
        public string Title
        {
            get { return _Title; }
        }

        public string _Path;
        public string Path
        {
            get { return _Path; }
        }

        private float IndexerScore;
        private int _MatchPages;
        public int MatchPages
        {
            get { return _MatchPages; }
        }
        public float TotalScore
        {
            get { return IndexerScore + MatchPages * 100; }
        }

        // Dictionary<Page, Score>
        public Dictionary<int, float> Documents;

        public DocumentGroup(string title, string path)
        {
            _Title = title;
            _Path = path;
            IndexerScore = 0;
            _MatchPages = 0;
            Documents = new Dictionary<int, float>();
        }

        public void Add(int page, ScoreDoc scoreDoc)
        {
            // 중복 페이지가 들어오면, 점수가 높은 걸로 저장
            // 중복 페이지가 들어오는 경우 --> OCR 데이터 또는 콘텐츠 데이터
            if (Documents.ContainsKey(page))
            {
                Documents.TryGetValue(page, out float storedScore);
                if (scoreDoc.Score > storedScore)
                {
                    Documents.Remove(page);
                    Documents.Add(page, scoreDoc.Score);

                    IndexerScore = IndexerScore - storedScore + scoreDoc.Score;
                }
            } else
            {
                Documents.Add(page, scoreDoc.Score);
                IndexerScore += scoreDoc.Score;
            }

            _MatchPages++;
        }
    }
}
