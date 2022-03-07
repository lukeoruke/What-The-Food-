using Console_Runner;
using Console_Runner.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Console_Runner.AMRModel;

namespace Console_Runner.User_Management
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


        public bool IsActive { get; set; }

        public bool Enabled { get; set; }

        public string Password { get; set; }

        public AMR? AMR { get; set; }
        public Account()
        {
            Enabled = true;
            Fname = "";
            Lname = "";
        }
        public override string ToString()
        {
            int pass = this.Password.Length;
            string stars = "";
            for(int i = 0; i < pass; i++)
            {
                stars += "*";
            }
            return this.Email + " " + this.Fname + " " + this.Lname + " " + stars;
        }


    }
    
}

//add-migration CreateCustomerDB
//update-database -verbose