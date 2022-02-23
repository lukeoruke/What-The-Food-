using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAndArchive
{
    public interface ILogger
    {
        public bool log(string toLog);
        public bool logLogin(string category, string pageName, bool isSuccess, string failCase, string user);
        public bool logAccountDeactivation(string category, string pageName, bool isSuccess, string failCase, string user, string target);
        public bool logAccountEnabling(string category, string pageName, bool isSuccess, string failCase, string user, string target);
        public bool logAccountPromote(string category, string pageName, bool isSuccess, string failCase, string user, string promoted);
        public bool logAccountCreation(string category, string pageName, bool isSuccess, string failCase, string user);
        public bool logAccountDeletion(string category, string pageName, bool isSuccess, string failCase, string user);
        public bool logAccountNameChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevName, string newName);
        public bool logAccountEmailChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevEmail, string newEmail);
        public bool logAccountPasswordChange(string category, string pageName, bool isSuccess, string failCase, string user);
        public bool logAccountFlagChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] added, string[] removed);
        public bool logAccountDataRequest(string category, string pageName, bool isSuccess, string failCase, string user, string sendTo);
        public bool logAccountAmrChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] from, string[] to);
        public bool logReview(string category, string pageName, bool isSuccess, string failCase, string user, string product, int rating, string text);
        public bool logHistory(string category, string pageName, bool isSuccess, string failCase, string user, string product, int index);
        public bool logScanUpload(string category, string pageName, bool isSuccess, string failCase, string user, string product);
        public bool logGeneric(string category, string pageName, bool isSuccess, string failCase, string user, string info);
    }
}
