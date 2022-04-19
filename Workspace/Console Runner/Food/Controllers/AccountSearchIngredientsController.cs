using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Console_Runner.FoodService;
using Console_Runner.AccountService;
using Console_Runner.Logging;

namespace Food.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountSearchIngredientsController : ControllerBase
    {
        private const string UM_CATEGORY = "Data Store";

        private readonly IFoodGateway _foodGateway = new EFFoodGateway();
        [HttpGet]
        public async Task<ActionResult<string>> GET()
        {
            FoodDBOperations _foodDBOperations = new FoodDBOperations(_foodGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;

            try
            {
                string input = Request.QueryString.Value;
                Console.WriteLine("(accountSearchController)input: " + input);
                string[] inputarr = input.Split('?');
                string search = inputarr[1];

                string page = inputarr[2];
                int numberOfItemsDisplayedAtOnce = 1;
                Console.WriteLine("GET " + search);
                var allIngredientList = await _foodDBOperations.GetIngredientBySearchAsync(search, numberOfItemsDisplayedAtOnce * int.Parse(page)
                    , numberOfItemsDisplayedAtOnce, logger);
                Console.WriteLine("Length of ing list(search function) = " + allIngredientList.Count());
                string jsonStr = "{";
                
                jsonStr += FormatIngredientsJsonString(allIngredientList);
                Console.WriteLine(jsonStr);
                return jsonStr + "}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "Something went wrong getting the ingredients list to display on food flags page";
            }
        }
        public string FormatIngredientsJsonString(List<Ingredient> ingredientList)
        {
            string strNameList = "\"IngredientName\": [";

            string strIngIDList = "\"IngredientID\": [";
            for (int i = 0; i < ingredientList.Count; i++)
            {
                strNameList += $"\"{ingredientList[i].IngredientName}\"";
                strIngIDList += $"\"{ingredientList[i].IngredientID}\"";

                if (i < ingredientList.Count - 1)
                {
                    strNameList += ",";

                    strIngIDList += ",";
                }
                else if (i == ingredientList.Count - 1)
                {
                    strNameList += "]";
                    strIngIDList += "]";
                }
            }

            return strNameList + ", " + strIngIDList;
        }
    }
}



