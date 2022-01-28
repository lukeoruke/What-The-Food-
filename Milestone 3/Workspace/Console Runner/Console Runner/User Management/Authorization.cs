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

    public class Authorization
    {
       // [Keyless]
        
        public class user_permissions
        {
            [System.ComponentModel.DataAnnotations.Key]
            public String email{ get; set; }
            public String permission { get; set; }
           // public List<Account> acc { get; set; }
            public user_permissions()
            {
                this.email = "IF THIS SHOWS UP, YOU GOT A BUG.";
            }
            public user_permissions(String email, String permission)
            {
                this.email = email;
                this.permission = permission;

            }
        }
    }
}
