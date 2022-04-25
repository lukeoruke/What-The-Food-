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
        public async Task<bool> DecrementBias(int userID) //Look up async and task
        {
            try
            {
                //Find and assign instance of userID within database
                Account temp = _efContext.Accounts.Where(a => a.UserID == userID).FirstOrDefault();
                if (temp == null)
                {
                    throw new Exception("ERROR: temp is null in decrementBias from EFNews");
                }
                if (temp.NewsBias >= 0)
                {
                    temp.NewsBias = temp.NewsBias - 1;
                    _efContext.Update(temp.NewsBias);
                    _efContext.Accounts.Update(temp);
                    await _efContext.SaveChangesAsync(); //await so that all changes can be made before saving
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public async Task<bool> IncrementBias(int userID)
        {
            try
            {
                //Find and assign instance of userID within database
                Account temp = _efContext.Accounts.Where(a => a.UserID == userID).FirstOrDefault();
                if(temp == null)
                {
                    throw new Exception("ERROR: temp is null in incrementBias from EFNews");
                }
                if (temp.NewsBias <= 4)
                {
                    temp.NewsBias = temp.NewsBias + 1;
                    _efContext.Update(temp.NewsBias);
                    _efContext.Accounts.Update(temp);
                    await _efContext.SaveChangesAsync(); //await so that all changes can be made before saving
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public async Task<int> GetBias(int userID)
        {
            try
            {
                //Find and assign instance of userID within database
                Account temp = _efContext.Accounts.Where(a => a.UserID == userID).FirstOrDefault();
                if (temp == null)
                {
                    throw new Exception("ERROR: temp is null in incrementBias from EFNews");
                }
                return temp.NewsBias;
            }
            catch (Exception e)
            {
                throw new Exception("ERROR: temp is null in getBias from EFNews: " + e.Message);
            }

        }
    }
}
