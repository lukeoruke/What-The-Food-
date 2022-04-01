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

        //base logging function that will write to the log database defined in ContectLoggingDB.cs
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