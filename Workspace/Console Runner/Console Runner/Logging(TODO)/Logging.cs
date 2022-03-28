namespace Console_Runner.Logging
{
    public class Logging
    {
        private ILogGateway _logAccess;
        private IUserIDGateway _userIDAccess;
        //logging objects
        public Logging(ILogGateway logAccessor, IUserIDGateway uidAccessor)
        {
            _logAccess = logAccessor;
            _userIDAccess = uidAccessor;
        }

        //base logging function that will write to the log.txt file. Will append logging information to the end of current date and time.
        public bool Log(string actorID, LogLevel level, Category category, DateTime timestamp, string message)
        {
            string? userHash = _userIDAccess.GetUserHash(actorID);
            if (userHash == null)
            {
                userHash = _userIDAccess.AddUserId(actorID);
            }
            Log record = new Log(userHash, level, category, timestamp.ToUniversalTime(), message);
            _logAccess.WriteLog(record);
            return true;
        }
    }
}