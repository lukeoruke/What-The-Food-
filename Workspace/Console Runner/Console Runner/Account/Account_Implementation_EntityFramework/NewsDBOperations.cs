using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Console_Runner.Account.AccountInterfaces;
namespace Console_Runner.Account.Account_Implementation_EntityFramework
{
    public class NewsDBOperations
    {
        private IAccountNews _newsGateway; 
        public NewsDBOperations(IAccountNews newsAccess)
        {
            _newsGateway = newsAccess;
        }
        public async Task<int> GetHealthBias(int userId)
        {
           return await  _newsGateway.GetBias(userId);
        } 

        public async Task<bool> IncrementHealthNews(int userId)
        {
            return await _newsGateway.IncrementBias(userId);
        }
        public async Task<bool> DecrementHealthNews(int userId)
        {
            return await _newsGateway.DecrementBias(userId);
        }
    }
}
