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
        //constructor
        public NewsDBOperations(IAccountNews newsAccess)
        {
            _newsGateway = newsAccess;
        }
        /// <summary>
        /// Returns the number of health news account currently is set at
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>returns the number of health news</returns>
        public async Task<int> GetHealthBias(int userId)
        {
           return await  _newsGateway.GetBias(userId);
        }
        /// <summary>
        /// increments the number of health news to be displayed
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>returns status of if health news filter is incremented</returns>
        public async Task<bool> IncrementHealthNews(int userId)
        {
            return await _newsGateway.IncrementBias(userId);
        }
        /// <summary>
        /// decrements the number of health news to be displayed
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>returns status of if health news filter is decremented</returns>
        public async Task<bool> DecrementHealthNews(int userId)
        {
            return await _newsGateway.DecrementBias(userId);
        }
    }
}
