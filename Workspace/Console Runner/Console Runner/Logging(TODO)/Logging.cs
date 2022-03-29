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

        //base logging function that will write to the log database defined in ContectLoggingDB.cs
        public async Task<bool> WriteLogAsync(string actorID, LogLevel level, Category category, DateTime timestamp, string message, int timeout = 0)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                var token = cts.Token;
                if(timeout > 0)
                {
                    cts.CancelAfter(timeout);
                }
                string? userHash = await _userIDAccess.GetUserHashAsync(actorID, token);
                if (userHash == null)
                {
                    userHash = await _userIDAccess.AddUserIdAsync(actorID, token);
                }
                Log record = new Log(userHash, level, category, timestamp.ToUniversalTime(), message);
                await _logAccess.WriteLogAsync(record, token);
                return true;
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