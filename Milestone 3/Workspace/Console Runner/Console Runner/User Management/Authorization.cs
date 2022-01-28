﻿using Class1;
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
        public string email{ get; set; }
        public string permission { get; set; }

        private Context context = new Context();
        public user_permissions()
        {
            email = "";
            permission = "";

        }
        public void setUserPermissions(string email, string permission)
        {
            this.email = email;
            this.permission = permission;
        }
        public void defualtUserPermissions(string email)
        {
            addPermission(email, "scanFood");
            addPermission(email, "editOwnAccount");
            addPermission(email, "leaveReview");
            addPermission(email, "deleteOwnAccount");
            addPermission(email, "historyAccess");
            addPermission(email, "AMR");
            addPermission(email, "foodFlag");
        }

        public void defualtAdminPermissions(string email)
        {
            
            defualtUserPermissions(email);
            addPermission(email, "enableAccount");
            addPermission(email, "disableAccount");
            addPermission(email, "deleteAccount");
            addPermission(email, "createAdmin");
        }

        public void addPermission(string email, string permission)
        {
            user_permissions newPermission = new user_permissions();
            newPermission.setUserPermissions(email, permission);
            context.permissions.Add(newPermission);
            context.SaveChanges();
        }

        //Email of the user being checked, permission name being checked
        public bool hasPermission(string email, string permission)
        {
            if (context.permissions.Find(email, permission) != null)
            {
                return true;
            }
            return false;
            
        }


    }


}
