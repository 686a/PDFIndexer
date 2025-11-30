using System;
using System.Collections.Generic;
using System.Diagnostics;
using PDFIndexer.Journal;

public delegate void MessageEventHandler(JournalMessage log);

namespace PDFIndexer.Journal
{
    /// <summary>
    /// 디버그 로깅을 위한 전용 클래스
    /// 
    /// 디버그 모드가 아닐 시, 리소스 절약을 위해 아래 메서드들은 아무 작업을 수행하지 않음.
    /// </summary>
    internal class Logger
    {
        private static readonly List<JournalMessage> Logs = new List<JournalMessage>();

        /// <summary>
        /// 저널 로그가 추가될 때 발생합니다.
        /// </summary>
        public static event MessageEventHandler OnMessage;

        public static JournalMessage[] RetrieveRecentLogs()
        {
#if DEBUG
            int from = Math.Max(0, Logs.Count - 30);
            int to = Logs.Count;
            return Logs.GetRange(from, to).ToArray();
#endif
        }

        public static void Write(string message)
        {
#if DEBUG
            Write(JournalLevel.Debug, message);
#endif
        }

        public static void Write(JournalLevel level, string message)
        {
#if DEBUG
            var log = new JournalMessage(level, message);
            Logs.Add(log);
            Debug.WriteLine($"[{level}] {message}");
            OnMessage?.Invoke(log);
#endif
        }

        public static void Clear()
        {
#if DEBUG
            Debug.WriteLine($"Journal log cleared.");
            Logs.Clear();
#endif
        }
    }
}
