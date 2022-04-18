

namespace Console_Runner.FoodService;

public class FoodItem
{
    public string Barcode { get; set; }
    public string ProductName { get; set; }
    public string CompanyName { get; set; }
    public string ProductPic { get; set; }
    //public string labelID { get; set; }
    public FoodItem()
    {

    }
    public FoodItem(string barcode, string productName, string companyName, string pic)
    {
        Barcode = barcode;
        ProductName = productName;
        CompanyName = companyName;
        ProductPic = pic;
    }
   
    public string FormatJsonString()
    {
        return $"\"Barcode\": \"{Barcode}\", \"ProductName\": \"{ProductName}\"," +
            $" \"CompanyName\": \"{CompanyName}\", \"ProductPic\": \"{ProductPic}\"";
    }
}
