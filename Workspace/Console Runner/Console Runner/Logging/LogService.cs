namespace Console_Runner.Logging
{
    public class LogService
    {
        private ILogGateway _logAccess;
        private IUserIDGateway _userIDAccess;
        //logging objects
        public LogService(ILogGateway logAccessor, IUserIDGateway uidAccessor)
        {
            _logAccess = logAccessor;
            _userIDAccess = uidAccessor;
        }

        /// <summary>
        /// Writes a Log entry to the Log database through _logAccess.
        /// </summary>
        /// <param name="actorID">The ID of the user performing the action to be logged.</param>
        /// <param name="level">The Log Level (Info, Debug, Warning, Error) of the action being logged.</param>
        /// <param name="category">The category (View, Business, Server, Data, DataStore) of the action being logged.</param>
        /// <param name="timestamp">The time and date that the action to be logged occured.</param>
        /// <param name="message">A string containing a message to be included with the log entry.</param>
        /// <param name="timeout">The time in milliseconds that are allowed to elapse before the log attempt is considered failed.</param>
        /// <returns>A Log object representing the log entry to be written to the database.</returns>
        public async Task<Log> WriteLogAsync(string actorID, LogLevel level, Category category, DateTime timestamp, string message, int timeout = -1)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                var token = cts.Token;
                if(timeout > -1)
                {
                    cts.CancelAfter(timeout);
                }
                string? userHash = await _userIDAccess.GetUserHashAsync(actorID, token);
                if (userHash == null)
                {
                    userHash = await _userIDAccess.AddUserIdAsync(actorID, token);
                }
                Console.WriteLine("writing log...");
                Log record = new Log(userHash, level, category, timestamp.ToUniversalTime(), message);
                await _logAccess.WriteLogAsync(record, token);
                return record;
            }
            catch (OperationCanceledException ex)
            {
                throw (ex);
            }
            finally
            {
                cts.Dispose();
            }
        }
    }
}