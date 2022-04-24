

namespace Console_Runner.FoodService;

public class Review
{
    public string Barcode { get; set; }
    public string UserID { get; set; }
    public string UserRating { get; set;}
    public string UserReview { get; set; }

    public Review()
    {

    }
    
    public Review(string barcode, string UID, string rating, string review)
    {
        Barcode = barcode;
        UserID = UID;
        UserRating = rating;
        UserReview = review;
    }
}
