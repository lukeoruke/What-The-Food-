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
        public IPermissionGateway dataAccess { get;}

        public user_permissions()
        {

        }
        public user_permissions(IPermissionGateway dal)
        {
            this.dataAccess = dal;
            this.email = "";
            this.permission = "";

        }
        public user_permissions(string email, string permission, IPermissionGateway dal)
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
        public void AssignDefaultUserPermissions(string email)
        {

            dataAccess.AddPermission(email, "scanFood");
            dataAccess.AddPermission(email, "editOwnAccount");
            dataAccess.AddPermission(email, "leaveReview");
            dataAccess.AddPermission(email, "deleteOwnAccount");
            dataAccess.AddPermission(email, "historyAccess");
            dataAccess.AddPermission(email, "AMR");
            dataAccess.AddPermission(email, "foodFlag");

        }
        /* contains a package of the defualt permissions that will be assigned to all new admin accounts.
         * Email: the PK of the account we are giving these permissions to
         */
        public void AssignDefaultAdminPermissions(string email)
        {

            //defualtUserPermissions(email);
            dataAccess.AddPermission(email, "enableAccount");
            dataAccess.AddPermission(email, "disableAccount");
            dataAccess.AddPermission(email, "deleteAccount");
            dataAccess.AddPermission(email, "createAdmin");
            dataAccess.AddPermission(email, "editOtherAccount");
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