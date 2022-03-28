namespace Console_Runner.Logging
{
    public enum LogLevel
    {
        Info,
        Debug,
        Warning,
        Error
    }

    public enum Category
    {
        View,
        Business,
        Server,
        Data,
        DataStore
    }
    /*
     * Represents a single entry on the Logs database table.
     */
    public class Log
    {
        public int LogId { get; }
        public string UserIdentifier { get; }
        public LogLevel LogLevel { get; }
        public Category Category { get; }
        public DateTime Timestamp { get; }
        public string Message { get; }

        public Log(string uid, LogLevel level, Category category, DateTime timestamp, string message)
        {
            UserIdentifier = uid;
            LogLevel = level;
            Category = category;
            Timestamp = timestamp;
            Message = message;
        }

        public override string ToString()
        {
            return $"User: {UserIdentifier}\nLevel: {LogLevel}\nCategory: {Category}\nTimestamp: {Timestamp}\nMessage: {Message}";
        }
    }
}

//add-migration CreateCustomerDB
//update-database -verbose