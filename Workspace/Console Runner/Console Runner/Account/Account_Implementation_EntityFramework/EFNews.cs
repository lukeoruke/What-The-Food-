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
                Account acc = _efContext.Accounts.Where(a => a.UserID == userID).FirstOrDefault();
                Console.WriteLine("NEWS BIAS AT " + acc.NewsBias + "\n");
                Console.Write("Ran in decrement Bias \n");
                if (acc == null)
                {
                    throw new Exception("ERROR: temp is null in decrementBias from EFNews");
                }
                if (acc.NewsBias > 1)
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

        public async Task<bool> IncrementBias(int userID)
        {
            try
            {
                //Find and assign instance of userID within database
                Account acc = _efContext.Accounts.Where(a => a.UserID == userID).FirstOrDefault();
                Console.WriteLine("NEWS BIAS AT " + acc.NewsBias + "\n");
                Console.Write("Ran in increment Bias \n");
                if (acc == null)
                {
                    throw new Exception("ERROR: temp is null in incrementBias from EFNews");
                }
                if (acc.NewsBias < 4)
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
