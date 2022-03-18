using Console_Runner;
using Console_Runner.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Console_Runner.AMRModel;

namespace Console_Runner.Account
{
    /*
     * Account class that will represent the contents of a user's account
     */
    public class Account { 
        //User ID getter and setter
        //Using Auto Incrementer, research on that
        //[System.ComponentModel.DataAnnotations.Key]
        //public string UserID{ get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //User's first name
        public string FName { get; set; }
        //User's last name
        public string LName { get; set; }

        //TODO: Constructor values?
        public Account()
        {
            //UserID = "";
            //UserID = "";
            FName = "";
            LName = "";
        }

        public override string ToString()
        {
            int pass = this.Password.Length;
            string stars = "";
            for (int i = 0; i < pass; i++)
            {
                stars += "*";
            }
            return this.Email + " " + this.FName + " " + this.LName + " " + stars;
        }
    }
}
