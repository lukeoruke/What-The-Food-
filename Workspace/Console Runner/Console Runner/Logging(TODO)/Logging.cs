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
        public async Task<bool> WriteLogAsync(string actorID, LogLevel level, Category category, DateTime timestamp, string message)
        {
            string? userHash = await _userIDAccess.GetUserHashAsync(actorID);
            if (userHash == null)
            {
                userHash = await _userIDAccess.AddUserIdAsync(actorID);
            }
            Log record = new Log(userHash, level, category, timestamp.ToUniversalTime(), message);
            await _logAccess.WriteLogAsync(record);
            return true;
        }
    }
}