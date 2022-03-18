using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.DAL.Implementation_InMemory
{
    public class MemHistoryGateway : IHistoryGateway
    {
        private List<History> _historyDB;

        public MemHistoryGateway()
        {
            _historyDB = new List<History>();
        }
        public bool AddHistoryItem(History historyItem)
        {
            _historyDB.Add(historyItem);
            return true;
        }
    }
}
