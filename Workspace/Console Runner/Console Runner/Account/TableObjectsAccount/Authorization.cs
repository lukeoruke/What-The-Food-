
using System.ComponentModel.DataAnnotations.Schema;
namespace Console_Runner.AccountService
{
    public class Authorization
    {
        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public string Permission { get; set; }

        public Authorization()
        {

        }

        public Authorization(int userID, string resource)
        {
            UserID = userID;
            Permission = resource;
        }
    }
}
