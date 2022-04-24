

namespace Console_Runner.AccountService
{
    /*
     * Account class that will represent the contents of a user's account
     */
    public class Account {
        //User ID getter and setter
        //Using Auto Incrementer, research on that
        //[System.ComponentModel.DataAnnotations.Key]
        public int UserID{ get; set; }
        
        public string Email { get; set; }
        public string Password { get; set; }
        //User's first name
        public string FName { get; set; }
        //User's last name
        public string LName { get; set; }

        public bool IsActive { get; set; }

        public bool Enabled { get; set; }

        public string salt { get; set; }
        //TODO: Constructor values?
        
        public Account()
        {
            Email = "";
            Password = "";
        }

        public override string ToString()
        {
            int pass = this.Password.Length;
            string stars = "";
            for (int i = 0; i < pass; i++)
            {
                stars += "*";
            }
            return this.Email + " " + this.FName + " " + this.LName + " " + stars;
        }
    }
}
