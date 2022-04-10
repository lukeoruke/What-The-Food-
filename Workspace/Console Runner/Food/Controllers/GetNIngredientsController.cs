
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;


    using Console_Runner.FoodService;
    namespace Food.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class GetNIngredientsController : ControllerBase
        {
            private const string UM_CATEGORY = "Data Store";

            private readonly IFoodGateway _foodGateway = new EFFoodGateway();
            [HttpGet]
            public async Task<ActionResult<string>> GET()
            {
                FoodDBOperations _foodDBOperations = new FoodDBOperations(_foodGateway);
                string page = Request.QueryString.Value;
                page = page.Substring(1);
            try
            {
                Console.WriteLine("Page " + page);
                var allIngredientList = await _foodDBOperations.GetNIngredients(5*int.Parse(page), 5);
                Console.WriteLine("Length of ing list = " + allIngredientList.Count());
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
