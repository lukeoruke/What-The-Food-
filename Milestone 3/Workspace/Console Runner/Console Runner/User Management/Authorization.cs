using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner
{
    public class Authorization
    {
        public class Role_User
        {
            [System.ComponentModel.DataAnnotations.Key]
            public int accessLevel { get; set; }
            protected bool scanAccess { get; set; }
            protected bool editOwnAccount { get; set; }
            protected bool editOtherAccount { get; set; }
            protected bool promotAdmin { get; set; }

            public Role_User()
            {
                accessLevel = 1;
                scanAccess = true;
                editOwnAccount = true;
                editOtherAccount = false;
                promotAdmin = false;
            }


            public string ToString()
            {
                return "Permission Level: " + accessLevel.ToString() + " \nHasScanPermission: " + scanAccess.ToString() + " \nHasEditOwnAccountPermission: " + editOwnAccount.ToString() + " \nHasEditOtherAccountPermission: " + editOtherAccount.ToString() + " \nHasPromotToAdminPermission: " + promotAdmin.ToString() + "\n";
            }
        }
        public class Role_Admin : Role_User
        {
            public Role_Admin()
            {
                this.accessLevel = 2;
                this.scanAccess = true;
                this.editOwnAccount = true;
                this.editOtherAccount = true;
                this.promotAdmin = true;
            }
        }
    }
}
