

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Console_Runner.Logging
{
    public class EFLogGateway : ILogger
    {
        /*private readonly Context _efContext;

        public EFLogGateway()
        {
            _efContext = new Context();
        }

        public bool WriteLog(Logs toLog)
        {
            _efContext.Logs.Add(toLog);
            return true;
        }*/
        public bool Log(string toLog)
        {
            throw new NotImplementedException();
        }

        public bool LogAccountAmrChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] from, string[] to)
        {
            throw new NotImplementedException();
        }

        public bool LogAccountCreation(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            throw new NotImplementedException();
        }

        public bool LogAccountDataRequest(string category, string pageName, bool isSuccess, string failCase, string user, string sendTo)
        {
            throw new NotImplementedException();
        }

        public bool LogAccountDeactivation(string category, string pageName, bool isSuccess, string failCase, string user, string target)
        {
            throw new NotImplementedException();
        }

        public bool LogAccountDeletion(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            throw new NotImplementedException();
        }

        public bool LogAccountEmailChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevEmail, string newEmail)
        {
            throw new NotImplementedException();
        }

        public bool LogAccountEnabling(string category, string pageName, bool isSuccess, string failCase, string user, string target)
        {
            throw new NotImplementedException();
        }

        public bool LogAccountFlagChange(string category, string pageName, bool isSuccess, string failCase, string user, string[] added, string[] removed)
        {
            throw new NotImplementedException();
        }

        public bool LogAccountNameChange(string category, string pageName, bool isSuccess, string failCase, string user, string prevName, string newName)
        {
            throw new NotImplementedException();
        }

        public bool LogAccountPasswordChange(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            throw new NotImplementedException();
        }

        public bool LogAccountPromote(string category, string pageName, bool isSuccess, string failCase, string user, string promoted)
        {
            throw new NotImplementedException();
        }

        public bool LogGeneric(string category, string pageName, bool isSuccess, string failCase, string user, string info)
        {
            throw new NotImplementedException();
        }

        public bool LogHistory(string category, string pageName, bool isSuccess, string failCase, string user, string product, int index)
        {
            throw new NotImplementedException();
        }

        public bool LogLogin(string category, string pageName, bool isSuccess, string failCase, string user)
        {
            throw new NotImplementedException();
        }

        public bool LogReview(string category, string pageName, bool isSuccess, string failCase, string user, string product, int rating, string text)
        {
            throw new NotImplementedException();
        }

        public bool LogScanUpload(string category, string pageName, bool isSuccess, string failCase, string user, string product)
        {
            throw new NotImplementedException();
        }
    }
}
