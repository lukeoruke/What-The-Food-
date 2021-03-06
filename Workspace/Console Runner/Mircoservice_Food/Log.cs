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
        public int LogId { get; set; }
        public UserIdentifier UserIdentifier { get; set; } = null!;
        public LogLevel LogLevel { get; set; }
        public Category Category { get; set; }
        public DateTime Timestamp { get; set; }
        public string CallSiteFile { get; set; }
        public string CallSiteMethod { get; set; }
        public string Message { get; set; }

        public Log()
        {

        }
        public Log(UserIdentifier userId, LogLevel logLevel, Category category, string callSiteFile, string callSiteMethod, DateTime timestamp, string message)
        {
            UserIdentifier = userId;
            LogLevel = logLevel;
            Category = category;
            CallSiteFile = callSiteFile;
            CallSiteMethod = callSiteMethod;
            Timestamp = timestamp.ToUniversalTime();
            Message = message;
        }

        public override string ToString()
        {
            return $"User: {UserIdentifier?.UserHash ?? "Unidentified", -20}\nLevel: {LogLevel, -10}\nCategory: {Category, -10}\nTimestamp: {Timestamp, -15}\nMessage: {Message}";
        }
    }
}

//add-migration CreateCustomerDB
//update-database -verbose