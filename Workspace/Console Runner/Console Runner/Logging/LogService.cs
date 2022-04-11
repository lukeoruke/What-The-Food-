using Console_Runner.Logging.Testing;
namespace Console_Runner.Logging
{
    public static class LogService
    {
        //TODO: read DataStoreType as a value from a config file instead of hardcoding
        private const string DataStoreType = "EF";
        private static ILogGateway _logAccess;
        private static IUserIDGateway _userIDAccess;
        //logging objects
        static LogService()
        {
            if(DataStoreType == "EF")
            {
                _logAccess = new EFLogGateway();
                _userIDAccess = new EFUserIdentifierGateway();
            }
            else
            {
                _logAccess = new MemLogGateway();
                _userIDAccess = new MemUserIdentifierGateway();
            }
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
        public static async Task<Log> WriteLogAsync(string actorID, LogLevel level, Category category, DateTime timestamp, string message, int timeout = -1)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                var token = cts.Token;
                if(timeout > -1)
                {
                    cts.CancelAfter(timeout);
                }
                UserIdentifier? uid = await _userIDAccess.GetUserIdentifierAsync(actorID, token);
                if(uid == null)
                {
                    uid = await _userIDAccess.AddUserIdAsync(actorID, token);
                }
                Log record = new Log(uid, level, category, timestamp.ToUniversalTime(), message);
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