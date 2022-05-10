using System.Linq;

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
        public async Task<Log> LogAsync(string actorID, LogLevel level, Category category, DateTime timestamp, string message,
            [System.Runtime.CompilerServices.CallerFilePath] string callerFile = "unknown",
            [System.Runtime.CompilerServices.CallerMemberName] string callerMethod = "unknown",
            int timeout = -1)
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
                Log record = new Log(uid, level, category, Path.GetFileName(callerFile), callerMethod, timestamp.ToUniversalTime(), message);
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
        public async Task<Log> LogWithSetUserAsync(LogLevel level, Category category, DateTime timestamp, string message,
            [System.Runtime.CompilerServices.CallerFilePath] string callerFile = "unknown",
            [System.Runtime.CompilerServices.CallerMemberName] string callerMethod = "unknown",
            int timeout = -1)
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
                Log record = new Log(uid, level, category, Path.GetFileName(callerFile), callerMethod, timestamp.ToUniversalTime(), message);
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

        public async Task<bool> LogListAsync(string actorID, IEnumerable<LogData> logsdata,
            [System.Runtime.CompilerServices.CallerFilePath] string callerFile = "unknown",
            [System.Runtime.CompilerServices.CallerMemberName] string callerMethod = "unknown",
            int timeout = -1)
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
                    toLog.Add(new Log(uid, data.LogLevel, data.Category, Path.GetFileName(callerFile), callerMethod, data.Timestamp.ToUniversalTime(), data.Message));
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

        public async Task<bool> LogListWithSetUserAsync(IEnumerable<LogData> logsdata,
            [System.Runtime.CompilerServices.CallerFilePath] string callerFile = "unknown",
            [System.Runtime.CompilerServices.CallerMemberName] string callerMethod = "unknown",
            int timeout = -1)
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
                    toLog.Add(new Log(uid, data.LogLevel, data.Category, Path.GetFileName(callerFile), callerMethod, data.Timestamp.ToUniversalTime(), data.Message));
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

        public Dictionary<DateTime, int> GetLogsAfterDate(DateTime dateToQuery)
        {
            List<Log> logsToParse = _logAccess.GetLogsWhere(log => log.Timestamp.ToUniversalTime() > dateToQuery.ToUniversalTime());
            Dictionary<DateTime, int> result = new Dictionary<DateTime, int>();
            // query logsToParse
            var loginsByDay = from log in logsToParse
                              // group list of logs by date
                              group log by log.Timestamp.Date into dateGroup
                              orderby dateGroup.Key
                              //get all groups
                              select dateGroup;
            foreach (var dategroup in loginsByDay)
            {
                result.Add(dategroup.Key, dategroup.Count());
            }
            return result;
        }

        
        public Dictionary<string, int> GetLoginTrends(DateTime since)
        {
            List<Log> loginLogs = _logAccess.GetLogsWhere((log) => (log.CallSiteFile == "AccountLoginController.cs") && (log.LogLevel == LogLevel.Info),
                                                          (log) => log.Timestamp > since);
            Dictionary<string, int> result = new Dictionary<string, int>();
            //
            var loginsByDay = from loginLog in loginLogs
                              group loginLog by loginLog.Timestamp.Date into dateGroup
                              orderby dateGroup.Key
                              select dateGroup;
            foreach(var dategroup in loginsByDay)
            {
                result.Add(dategroup.Key.ToShortDateString(), dategroup.Count());
            }
            return result;
        }

        public Dictionary<string, int> GetSignupTrends(DateTime since)
        {
            List<Log> signupLogs = _logAccess.GetLogsWhere((log) => (log.CallSiteFile == "AccountSignUpController.cs") && (log.CallSiteMethod == "Post"),
                                                           (log) => log.Timestamp > since);
            Dictionary<string, int> result = new();
            var signupsByDay = from signupLog in signupLogs
                               group signupLog by signupLog.Timestamp.Date into dateGroup
                               orderby dateGroup.Key
                               select dateGroup;
            foreach(var dategroup in signupsByDay)
            {
                result.Add(dategroup.Key.ToShortDateString(), dategroup.Count());
            }
            return result;
        }

        public Dictionary<string, int> GetMostViewedPages() {
            
            List<Log> viewLogs = _logAccess.GetLogsWhere((log) => (log.CallSiteFile == "ValidateLoggedInController.cs") && (log.Category == Category.View));
            Dictionary<string, int> result = new Dictionary<string, int>();

            var mostViewedPages = from viewLog in viewLogs                                
                                  select viewLog.Message;
            foreach(var messagegroup in mostViewedPages)
            {
                string viewName = "";
                string[] words = messagegroup.Split(' ');
                int fromIndex = Array.IndexOf(words, "from");
                viewName = words[fromIndex + 1];
                int currentCount = 0;
                result.TryGetValue(viewName, out currentCount);
                result[viewName] = currentCount + 1;
            }

            var myList = result.ToList();
            myList.Sort((entry1, entry2) => entry2.Value.CompareTo(entry1.Value));

            Dictionary<string, int> toReturn = new Dictionary<string, int>();
            if (myList.Count > 5)
            {
                toReturn = new Dictionary<string, int>(myList.GetRange(0, 5));
            }
            else
            {
                toReturn = new Dictionary<string, int>(myList);
            }
            return toReturn;
        }

        public Dictionary<string, int> GetHighestAverageDurationPages() {
            
            List<Log> viewLogs = _logAccess.GetLogsWhere((log) => (log.CallSiteFile == "ValidateLoggedInController.cs") && (log.Category == Category.View));
            Dictionary<string, (int, int)> result = new Dictionary<string, (int, int)>();

            var mostViewedPages = from viewLog in viewLogs                                
                                  select viewLog.Message;
            foreach(var messagegroup in mostViewedPages)
            {
                string[] words = messagegroup.Split(' ');
                int fromIndex = Array.IndexOf(words, "from");
                int afterIndex = Array.IndexOf(words, "after");
                if(fromIndex == -1 || afterIndex == -1)
                {
                    continue;
                }
                string viewName = words[fromIndex + 1];
                string duration = words[afterIndex + 1];
                int actualDuration = -1;
                Int32.TryParse(duration, out actualDuration);
                if(actualDuration == -1)
                {
                    continue;
                }
                var currentViewData = (0,0);
                result.TryGetValue(viewName, out currentViewData);
                currentViewData.Item1 = currentViewData.Item1 + 1;
                currentViewData.Item2 = currentViewData.Item2 + actualDuration;
                result[viewName] = currentViewData;
            }

            var myList = result.ToList();
            var myListAsAverageDurations = myList.ConvertAll<KeyValuePair<string, int>>((kvp) => new KeyValuePair<string, int>(kvp.Key, (kvp.Value.Item2/kvp.Value.Item1)));
            myListAsAverageDurations.Sort((entry1, entry2) => entry2.Value.CompareTo(entry1.Value));

            Dictionary<string, int> toReturn = new Dictionary<string, int>();
            if (myList.Count > 5)
            {
                toReturn = new Dictionary<string, int>(myListAsAverageDurations.GetRange(0, 5));
            }
            else
            {
                toReturn = new Dictionary<string, int>(myListAsAverageDurations);
            }
            return toReturn;
        }

        public Dictionary<string, int> GetMostScannedBarcodes()
        {
            List<Log> viewLogs = _logAccess.GetLogsWhere((log) => (log.CallSiteFile == "GetFoodProductFromBarCodeController.cs") && (log.Category == Category.Business)
                                                                  && (log.LogLevel == LogLevel.Info) && (log.CallSiteMethod == "GET"));
            Dictionary<string, int> result = new Dictionary<string, int>();
            
            foreach(Log log in viewLogs)
            {
                string[] words = log.Message.Split(" ");
                int barcodeIndex = Array.IndexOf(words, "barcode");
                if(barcodeIndex == -1)
                {
                    continue;
                }
                string barcode = words[barcodeIndex + 1];
                int currentCount = 0;
                result.TryGetValue(barcode, out currentCount);
                result[barcode] = currentCount + 1;
            }

            var myList = result.ToList();
            myList.Sort((entry1, entry2) => entry2.Value.CompareTo(entry1.Value));
            Dictionary<string, int> toReturn = new Dictionary<string, int>();
            if(myList.Count > 5)
            {
                toReturn = new Dictionary<string, int>(myList.GetRange(0, 5));
            }
            else
            {
                toReturn = new Dictionary<string, int>(myList);
            }
            return toReturn;
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