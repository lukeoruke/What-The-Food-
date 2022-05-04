namespace Console_Runner.AccountService
{
    public class ActiveSessionTracker
    {

        public int UserID;
        public string jwt;
        public string timeStamp;
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
