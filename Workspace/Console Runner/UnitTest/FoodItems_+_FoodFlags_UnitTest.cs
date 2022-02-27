using Console_Runner.DAL;
using Console_Runner.Food;
using LogAndArchive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class FoodItems___FoodFlags_UnitTest
    {
        public void addFlagToAccountSuccess()
        {
            IDataAccess dal = new DummyDaL();
            ILogger log = new Logging();
            FM fm = new FM(dal, log);
        }
    }
}
