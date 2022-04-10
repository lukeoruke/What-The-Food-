using Console_Runner.AccountService;
using Console_Runner.FoodService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

            int skip = 0;
            int take = 10;
            try
            {
                string search = Request.QueryString.Value;
                search = search.Substring(1);

                Console.WriteLine("GET " + search);
                var allIngredientList = await _foodDBOperations.GetIngredientBySearchAsync(search, skip, take);
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
            string strAltList = "\"IngredientAlternateName\": [";
            string strDescList = "\"IngredientDescription\": [";
            string strIngIDList = "\"IngredientID\": [";
            for (int i = 0; i < ingredientList.Count; i++)
            {

                strNameList += $"\"{ingredientList[i].IngredientName}\"";
                strAltList += $"\"{ingredientList[i].AlternateName}\"";
                strDescList += $"\"{ingredientList[i].IngredientDescription}\"";
                strIngIDList += $"\"{ingredientList[i].IngredientID}\"";

                if (i < ingredientList.Count - 1)
                {
                    strNameList += ",";
                    strAltList += ",";
                    strDescList += ",";
                    strIngIDList += ",";
                }
                else if (i == ingredientList.Count - 1)
                {
                    strNameList += "]";
                    strAltList += "]";
                    strDescList += "]";
                    strIngIDList += "]";
                }
            }

            return strNameList + ", " + strIngIDList;
        }
    }
}



