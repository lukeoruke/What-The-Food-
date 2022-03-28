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

        //formats string for user login to send to log()
        public bool LogLogin(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Login Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account deactivation to send to log()
        public bool LogAccountDeactivation(string category, string pageName, bool isSuccess, string failCase, string user, string target)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Deactivation Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Target Account: " + target);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account enabling to send to log()
        public bool LogAccountEnabling(string category, string pageName, bool isSuccess, string failCase, string user, string target)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Enabling Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Target Account: " + target);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account promoting to send to log()
        public bool LogAccountPromote(string category, string pageName, bool isSuccess, string failCase, string user, string promoted)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Promotion Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Account Promoted: " + promoted);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account creation to send to log()
        public bool LogAccountCreation(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Creation Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account deletion to send to log()
        public bool LogAccountDeletion(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Deletion Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account name change to send to log()
        public bool LogAccountNameChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevName, string newName)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Name Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + "Changed From: " + prevName + ": Changed To:" + newName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account email change to send to log()
        public bool LogAccountEmailChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevEmail, string newEmail)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Name Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + "Changed From: " + prevEmail + ": Changed To:" + newEmail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account password change to send to log()
        public bool LogAccountPasswordChange(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Password Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account food flag changes to send to log()
        public bool LogAccountFlagChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] added, string[] removed)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Account Flag Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Added Flags: " + added + " Removed Flags: " + removed);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account data request to send to log()
        public bool LogAccountDataRequest(string category, string pageName, bool isSuccess, string failCase, string user, string sendTo)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Data Request Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Send To: " + sendTo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for account AMR changes to send to log()
        public bool LogAccountAmrChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] from, string[] to)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": AMR Change Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " AMR Changed From: " + from + " AMR Changed To: " + to);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for reviews to send to log()
        public bool LogReview(string category, string pageName, bool isSuccess, string failCase, string user, string product, int rating, string text)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Review Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Product: " + product + " Rating: " + rating + " Review: ");
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for histroy changes to send to log()
        public bool LogHistory(string category, string pageName, bool isSuccess, string failCase, string user, string product, int index)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": History Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Product: " + product + " Index In History: " + index);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for scan additions to send to log()
        public bool LogScanUpload(string category, string pageName, bool isSuccess, string failCase, string user, string product)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Scan Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " Product: " + product);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //formats string for a generic logging event to send to log()
        public bool LogGeneric(string category, string pageName, bool isSuccess, string failCase, string user, string info)
        {
            try
            {
                Log("category: " + category + " " + pageName + ": Action Successful: " + isSuccess.ToString() + " " + failCase + ": User: " + user
                    + " " + info);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}