using Microsoft.AspNetCore.Mvc;

namespace Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetProductRatingController : ControllerBase
    {
        static string productName = "";

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            string[] totalRatings = await Scrape();

            string starRating = "";
            char star = '★';
            char halfStar = '⯪';
            char emptyStar = '☆';
            double rating = Math.Round(double.Parse(totalRatings[0]) * 2) / 2;
            starRating = new string(star, (int)rating);

            if (rating % 1 != 0)
            {
                starRating += halfStar;
            }

            if (starRating.Length < 5)
            {
                starRating += new string(emptyStar, 5 - starRating.Length);
            }

            string jsonStr = "{\"starRating\":\"" + starRating + "\", " + "\"rating\":\"" + totalRatings[0] + "\", " + "\"ratingCount\":\"" + totalRatings[1] + "\"}";
            Console.WriteLine(jsonStr);

            return jsonStr;
        }

        // Scrape ratings from external sites and average them
        [NonAction]
        public async Task<string[]> Scrape()
        {
            string[] totalRatings = new string[2];

            try
            {
                HttpClient client = new HttpClient();
                string[] amazonRatings = await GetAmazonRatings(productName, client);
                Console.WriteLine(amazonRatings[0]);
                Console.WriteLine(amazonRatings[1]);
                totalRatings[0] = amazonRatings[0];
                totalRatings[1] = amazonRatings[1];
            }
            catch (Exception e)
            {

            }
            return totalRatings;

        }

        [HttpPost]
        public async void Post()
        {
            try
            {
                IFormCollection formData = Request.Form;
                productName = formData["foodName"];
                Console.WriteLine(productName);
            }
            catch (Exception e)
            {

            }
        }

        // Function to get ratings of product from Amazon
        [NonAction]
        public async Task<string[]> GetAmazonRatings(String productName, HttpClient client)
        {
            string[] ratings = new string[2];
            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                // Connect to external site
                String url = "http://www.amazon.com/s?k=" + productName;
                string responseBody = await client.GetStringAsync(url);
                // Get product from search
                int index = responseBody.IndexOf("data-component-type=\"s-search-result\"");
                String asin = responseBody.Substring(index - 76, 10);
                // Get reviews from product
                url = "https://www.amazon.com/gp/customer-reviews/widgets/average-customer-review/popover/ref=dpx_acr_pop_?&asin=" + asin;
                responseBody = await client.GetStringAsync(url);
                // Get product rating
                index = responseBody.IndexOf("class=\"a-icon-alt\">") + 19;
                int endIndex = responseBody.IndexOf("</span>");
                int length = endIndex - index;
                String rating = responseBody.Substring(index, length);
                Console.WriteLine(rating);

                index = rating.IndexOf(" ");
                rating = rating.Substring(0, index);
                ratings[0] = rating;

                // Get product rating count
                index = responseBody.IndexOf("totalRatingCount") + 18;
                endIndex = responseBody.IndexOf("global ratings") + 14;
                length = endIndex - index;
                String ratingCount = responseBody.Substring(index, length);
                Console.WriteLine(ratingCount);

                index = ratingCount.IndexOf(" ");
                ratingCount = ratingCount.Substring(0, index);
                ratings[1] = ratingCount;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return ratings;
        }
    }
}

