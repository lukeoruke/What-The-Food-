using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Console_Runner.Logging
{
    /*
     * Account class that will represent the contents of a user's account
     */
    public class Logs
    {
        //Date getter
        public string Date { get;set; }

        //Date getter
        public string Time { get; set; }

        //Log getter
        public string Message { get; set; }

        public override string ToString()
        {
            return Date + " " + Time + " " + Message;
        }
    }
}

//add-migration CreateCustomerDB
//update-database -verbose