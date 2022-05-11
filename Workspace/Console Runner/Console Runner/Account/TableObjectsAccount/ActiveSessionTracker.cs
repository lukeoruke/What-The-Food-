using System.ComponentModel.DataAnnotations.Schema;

namespace Console_Runner.AccountService
{
    public class ActiveSessionTracker
    {
        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public string jwt { get; set; }
        public string timeStamp { get; set; }
        public ActiveSessionTracker()
        {

        }
        public ActiveSessionTracker(int userID, string jwt)
        {
            this.UserID = userID;
            this.jwt = jwt;
            this.timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(); ;
        }
    }
}
