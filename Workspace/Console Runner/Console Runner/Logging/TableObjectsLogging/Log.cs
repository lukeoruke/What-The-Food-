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
        public string ActorIdentifier { get; set; }
        public LogLevel LogLevel { get; set; }
        public Category Category { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        public Log()
        {

        }
        public Log(string actorIdentifier, LogLevel logLevel, Category category, DateTime timestamp, string message)
        {
            ActorIdentifier = actorIdentifier;
            LogLevel = logLevel;
            Category = category;
            Timestamp = timestamp.ToUniversalTime();
            Message = message;
        }

        public override string ToString()
        {
            return $"User: {ActorIdentifier}\nLevel: {LogLevel}\nCategory: {Category}\nTimestamp: {Timestamp}\nMessage: {Message}";
        }
    }
}

//add-migration CreateCustomerDB
//update-database -verbose