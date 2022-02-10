using Class1;
using Console_Runner.DAL;
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
        private IDataAccess dal;
        public user_permissions(IDataAccess dal)
        {
            this.dal = dal;
            email = "";
            permission = "";

        }
        public user_permissions(string email, string permission, IDataAccess dal)
        {
            this.dal = dal;
            this.email = email;
            this.permission = permission;

        }
        public void setUserPermissions(string email, string permission)
        {
            this.email = email;
            this.permission = permission;
        }
        /* contains a package of the defualt permissions that will be assigned to all new user accounts.
        * Email: the PK of the account we are giving these permissions to
        */
        public void defualtUserPermissions(string email)
        {
            
            dal.addPermission(email, "scanFood");
            dal.addPermission(email, "editOwnAccount");
            dal.addPermission(email, "leaveReview");
            dal.addPermission(email, "deleteOwnAccount");
            dal.addPermission(email, "historyAccess");
            dal.addPermission(email, "AMR");
            dal.addPermission(email, "foodFlag");

        }
        /* contains a package of the defualt permissions that will be assigned to all new admin accounts.
         * Email: the PK of the account we are giving these permissions to
         */
        public void defualtAdminPermissions(string email)
        {

            //defualtUserPermissions(email);
            dal.addPermission(email, "enableAccount");
            dal.addPermission(email, "disableAccount");
            dal.addPermission(email, "deleteAccount");
            dal.addPermission(email, "createAdmin");
            dal.addPermission(email, "editOtherAccount");
        }
    }


}


/*public void addPermission(string email, string permission)
{
    user_permissions newPermission = new user_permissions();
    newPermission.setUserPermissions(email, permission);
    Context context = new Context();
    if (context.permissions.Find(email, permission) == null)
    {
        context.permissions.Add(newPermission);
        context.SaveChanges();
    }

}*/