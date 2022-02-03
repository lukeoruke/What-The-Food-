using Console_Runner;
using Console_Runner.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace User
{

    /*
     * Account class that will represent the contents of a user's account
     */

    public class Account
    {
        //Email getter and setter
        [System.ComponentModel.DataAnnotations.Key] //set email as the PK
        public string Email { get; set; }
        //First name setter and getter
        public string Fname { get; set; }
        //Last name setter and getter
        public string Lname { get; set; }

        //public int accessLevel { get; set; }
        //Password getter and setter

        DaLWrapper dal = new();

        public bool isActive { get; set; }

        public bool enabled { get; set; }

        public string Password { get; set; }

        public Account()
        {
            enabled = true;
        }
        public override string  ToString()
        {
            int pass = this.Password.Length;
            string stars = "";
            for(int i = 0; i < pass; i++)
            {
                stars += "*";
            }
            return this.Email + " " + this.Fname + " " + this.Lname + " " + stars;
        }

        public bool isAdmin()
        {
            user_permissions permissions = new();
            return dal.hasPermission(Email, "createAdmin");

        }
        public bool isUser()
        {
            return !isAdmin();
        }

    }
    
}

//add-migration CreateCustomerDB
//update-database -verbose