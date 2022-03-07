using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Logging
{
    public interface ILogger
    {
        public bool Log(string toLog);
        public bool LogLogin(string category, string pageName, bool isSuccess, string failCase, string user);
        public bool LogAccountDeactivation(string category, string pageName, bool isSuccess, string failCase, string user, string target);
        public bool LogAccountEnabling(string category, string pageName, bool isSuccess, string failCase, string user, string target);
        public bool LogAccountPromote(string category, string pageName, bool isSuccess, string failCase, string user, string promoted);
        public bool LogAccountCreation(string category, string pageName, bool isSuccess, string failCase, string user);
        public bool LogAccountDeletion(string category, string pageName, bool isSuccess, string failCase, string user);
        public bool LogAccountNameChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevName, string newName);
        public bool LogAccountEmailChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevEmail, string newEmail);
        public bool LogAccountPasswordChange(string category, string pageName, bool isSuccess, string failCase, string user);
        public bool LogAccountFlagChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] added, string[] removed);
        public bool LogAccountDataRequest(string category, string pageName, bool isSuccess, string failCase, string user, string sendTo);
        public bool LogAccountAmrChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] from, string[] to);
        public bool LogReview(string category, string pageName, bool isSuccess, string failCase, string user, string product, int rating, string text);
        public bool LogHistory(string category, string pageName, bool isSuccess, string failCase, string user, string product, int index);
        public bool LogScanUpload(string category, string pageName, bool isSuccess, string failCase, string user, string product);
        public bool LogGeneric(string category, string pageName, bool isSuccess, string failCase, string user, string info);
    }
}
