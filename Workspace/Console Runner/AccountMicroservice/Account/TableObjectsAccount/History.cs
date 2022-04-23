using System.ComponentModel.DataAnnotations.Schema;

namespace Console_Runner.AccountService
{
    public class History
    {
        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public string Barcode { get; set; }
        //TODO: Implement UTC timestamp retrieval
        public long Timestamp { get; set; }
        public History() { }
        public History(int userID, string barcode)
        {
            this.UserID = userID;
            this.Barcode = barcode;
            //Current timestamp when creating a history object
            this.Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        public bool AddHistoryItem()
        {
            throw new NotImplementedException();
        }
    }
}
