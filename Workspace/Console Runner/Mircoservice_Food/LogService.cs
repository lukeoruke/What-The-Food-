namespace Console_Runner.Logging
{
    public class LogService
    {
        private ILogGateway _logAccess;
        private IUserIDGateway _userEmailAccess;
        public string? UserEmail { get; set; }
        public int? DefaultTimeOut { get; set; }
        //logging objects
        public LogService(ILogGateway logAccessor, IUserIDGateway userEmailAccessor)
        {
            _logAccess = logAccessor;
            _userEmailAccess = userEmailAccessor;
        }
        public LogService(ILogGateway logAccessor, IUserIDGateway userEmailAccessor, string userEmail)
        {
            _logAccess = logAccessor;
            _userEmailAccess = userEmailAccessor;
            UserEmail = userEmail;
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
        public async Task<Log> LogAsync(string actorID, LogLevel level, Category category, DateTime timestamp, string message, int timeout = -1)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                var token = cts.Token;
                if(timeout > -1)
                {
                    cts.CancelAfter(timeout);
                }
                else if(DefaultTimeOut != null)
                {
                    cts.CancelAfter((int)DefaultTimeOut);
                }
                token.ThrowIfCancellationRequested();
                UserIdentifier uid = await GetOrCreateUserID(actorID, token);
                Log record = new Log(uid, level, category, timestamp.ToUniversalTime(), message);
                token.ThrowIfCancellationRequested();
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

        /// <summary>
        /// Writes a Log entry to the Log database through _logAccess associated with the user ID this instance was instantiated with.
        /// </summary>
        /// <param name="level">The Log Level (Info, Debug, Warning, Error) of the action being logged.</param>
        /// <param name="category">The category (View, Business, Server, Data, DataStore) of the action being logged.</param>
        /// <param name="timestamp">The time and date that the action to be logged occured.</param>
        /// <param name="message">A string containing a message to be included with the log entry.</param>
        /// <param name="timeout">The time in milliseconds that are allowed to elapse before the log attempt is considered failed.</param>
        /// <returns>A Log object representing the log entry to be written to the database.</returns>
        public async Task<Log> LogWithSetUserAsync(LogLevel level, Category category, DateTime timestamp, string message, int timeout = -1)
        {
            if(UserEmail == null)
            {
                throw new InvalidOperationException("User ID was not set in LogService before calling LogWithSetUserAsync");
            }
            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                var token = cts.Token;
                if (timeout > -1)
                {
                    cts.CancelAfter(timeout);
                }
                else if (DefaultTimeOut != null)
                {
                    cts.CancelAfter((int)DefaultTimeOut);
                }
                token.ThrowIfCancellationRequested();
                UserIdentifier uid = await GetOrCreateUserID(UserEmail, token);
                Log record = new Log(uid, level, category, timestamp.ToUniversalTime(), message);
                token.ThrowIfCancellationRequested();
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

        public async Task<bool> LogListAsync(string actorID, IEnumerable<LogData> logsdata, int timeout = -1)
        {

            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                var token = cts.Token;
                if (timeout > -1)
                {
                    cts.CancelAfter(timeout);
                }
                token.ThrowIfCancellationRequested();
                UserIdentifier uid = await GetOrCreateUserID(actorID, token);
                List<Log> toLog = new();
                foreach (LogData data in logsdata)
                {
                    toLog.Add(new Log(uid, data.LogLevel, data.Category, data.Timestamp.ToUniversalTime(), data.Message));
                }
                token.ThrowIfCancellationRequested();
                await _logAccess.WriteLogsAsync(toLog, token);
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

        public async Task<bool> LogListWithSetUserAsync(IEnumerable<LogData> logsdata, int timeout = -1)
        {
            if (UserEmail == null)
            {
                throw new InvalidOperationException("User ID was not set in LogService before calling LogListWithSetUserAsync");
            }
            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                var token = cts.Token;
                if (timeout > -1)
                {
                    cts.CancelAfter(timeout);
                }
                token.ThrowIfCancellationRequested();
                UserIdentifier uid = await GetOrCreateUserID(UserEmail, token);
                List<Log> toLog = new();
                foreach (LogData data in logsdata)
                {
                    toLog.Add(new Log(uid, data.LogLevel, data.Category, data.Timestamp.ToUniversalTime(), data.Message));
                }
                token.ThrowIfCancellationRequested();
                await _logAccess.WriteLogsAsync(toLog, token);
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

        private async Task<UserIdentifier> GetOrCreateUserID(string uid, CancellationToken token = default)
        {
            UserIdentifier? identifier = await _userEmailAccess.GetUserIdentifierAsync(uid, token);
            if (identifier == null)
            {
                identifier = await _userEmailAccess.AddUserIdAsync(uid, token);
            }
            return identifier;
        }
    }
}