namespace PDFIndexer.Journal
{
    public readonly struct JournalMessage
    {
        public readonly JournalLevel Level;
        public readonly string Message;

        public JournalMessage(JournalLevel level, string message)
        {
            Level = level;
            Message = message;
        }
    }
}
