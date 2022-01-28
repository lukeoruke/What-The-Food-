using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User;

namespace Console_Runner
{


    
    public class user_permissions
    {
        //[System.ComponentModel.DataAnnotations.Key]
        public String email{ get; set; }
        //[System.ComponentModel.DataAnnotations.Key]
        public String permission { get; set; }
       // public List<Account> accounts { get; set; }
        // public List<Account> acc { get; set; }
        public user_permissions()
        {
            email = "";
            permission = "";
            //List<Account> accounts = new List<Account>();
            //this.email = "IF THIS SHOWS UP, YOU GOT A BUG.";
        }
        public void setUser_permissions(Account acc, String permission)
        {
            this.email = acc.Email;
            this.permission = permission;
            //accounts.Add(acc);

        }
    }
}
