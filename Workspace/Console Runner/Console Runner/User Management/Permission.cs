using Console_Runner.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.User_Management
{  
    public class Permission
    {
        public string Email{ get; set; }
        public string Resource { get; set; }

        public Permission()
        {

        }

        public Permission(string email, string resource)
        {
            Email = email;
            Resource = resource;
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