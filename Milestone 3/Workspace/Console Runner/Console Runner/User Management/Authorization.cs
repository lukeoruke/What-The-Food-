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

        [NotMapped]
        public IDataAccess dataAccess { get;}

        public user_permissions()
        {

        }
        public user_permissions(IDataAccess dal)
        {
            this.dataAccess = dal;
            this.email = "";
            this.permission = "";

        }
        public user_permissions(string email, string permission, IDataAccess dal)
        {
            this.dataAccess = dal;
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
        public void defaultUserPermissions(string email)
        {

            dataAccess.addPermission(email, "scanFood");
            dataAccess.addPermission(email, "editOwnAccount");
            dataAccess.addPermission(email, "leaveReview");
            dataAccess.addPermission(email, "deleteOwnAccount");
            dataAccess.addPermission(email, "historyAccess");
            dataAccess.addPermission(email, "AMR");
            dataAccess.addPermission(email, "foodFlag");

        }
        /* contains a package of the defualt permissions that will be assigned to all new admin accounts.
         * Email: the PK of the account we are giving these permissions to
         */
        public void defaultAdminPermissions(string email)
        {

            //defualtUserPermissions(email);
            dataAccess.addPermission(email, "enableAccount");
            dataAccess.addPermission(email, "disableAccount");
            dataAccess.addPermission(email, "deleteAccount");
            dataAccess.addPermission(email, "createAdmin");
            dataAccess.addPermission(email, "editOtherAccount");
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