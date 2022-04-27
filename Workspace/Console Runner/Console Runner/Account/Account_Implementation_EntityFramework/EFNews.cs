using Console_Runner.Account.AccountInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Console_Runner.AccountService
{
    public class EFNews : IAccountNews
    {
        private ContextAccountDB _efContext;

        //constructor
        public EFNews()
        {
            _efContext = new ContextAccountDB();
        }
        /// <summary>
        /// Decrements the bias by accessing account's attribute
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>return true if bias is decremented properly</returns>
        public async Task<bool> DecrementBias(int userID)
        {
            try
            {
                //Find and assign instance of userID within database
                Account acc = _efContext.Accounts.Where(a => a.UserID == userID).FirstOrDefault();
                Console.WriteLine("NEWS BIAS AT " + acc.NewsBias + "\n");
                Console.Write("Ran in decrement Bias \n");
                //if account is null throw an exception
                if (acc == null)
                {
                    throw new Exception("ERROR: account is null in decrementBias from EFNews");
                }
                //make sure we only decrement until 1 so that we can at least display 1 health news
                var healthNewsMin = 1;
                if (acc.NewsBias > healthNewsMin)
                {
                    acc.NewsBias = acc.NewsBias - 1;
                    Console.Write("Bias after decrement value: " + acc.NewsBias +"\n"); ;
                    _efContext.Accounts.Update(acc);
                    await _efContext.SaveChangesAsync(); //await so that all changes can be made before saving
                    return true;
                }
                else
                {
                    Console.WriteLine("Did not update because it is at min \n");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR IN EFNEWS Decrement" +e.Message);
                return false;
            }

        }
        /// <summary>
        /// Increments the bias by accessing account's attribute
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>return true if bias is incremented properly</returns>
        public async Task<bool> IncrementBias(int userID)
        {
            try
            {
                //Find and assign instance of userID within database
                Account acc = _efContext.Accounts.Where(a => a.UserID == userID).FirstOrDefault();
                Console.WriteLine("NEWS BIAS AT " + acc.NewsBias + "\n");
                Console.Write("Ran in increment Bias \n");
                //if account is null throw an exception
                if (acc == null)
                {
                    throw new Exception("ERROR: account is null in incrementBias from EFNews");
                }
                var healthNewsMax = 4;
                //Only incrememt news bias if it is not greater than 4 because we want to display at most 4 
                if (acc.NewsBias < healthNewsMax)
                {
                    acc.NewsBias = acc.NewsBias + 1;
                    Console.Write("Bias after increment value: " + acc.NewsBias +"\n");;
                    _efContext.Accounts.Update(acc);
                    await _efContext.SaveChangesAsync(); //await so that all changes can be made before saving
                    return true;
                }
                else
                {
                    Console.WriteLine("Did not update because it is at max \n");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR IN EFNEWS Increment:"+ e.Message);
                return false;
            }

        }
        /// <summary>
        /// return the bias by accessing account's attribute
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>returns the number of bias/health news that should be displayed</returns>
        public async Task<int> GetBias(int userID)
        {
            try
            {
                //Find and assign instance of userID within database
                Account temp = _efContext.Accounts.Where(a => a.UserID == userID).FirstOrDefault();
                //if account is null throw an exception
                if (temp == null)
                {
                    throw new Exception("ERROR: account is null in GetBias from EFNews");
                }
                return temp.NewsBias;
            }
            catch (Exception e)
            {
                throw new Exception("ERROR: Failed in getBias from EFNews: " + e.Message);
            }

        }
    }
}
