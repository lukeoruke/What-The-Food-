using Console_Runner.FoodService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Food.Controllers

{
    
    [Route("api/[controller]")]
    [ApiController]
    public class GetFoodProductFromBarCodeController : ControllerBase
    {
        private const string UM_CATEGORY = "Data Store";
        private readonly IFoodGateway _foodServiceGateway = new EFFoodGateway();
        private  FoodDBOperations _foodDB;
        private IFormCollection formData;
        private string barcode;
      

        [HttpGet]
        public async Task<ActionResult<string>> GET()
        {
            Console.WriteLine("This is the start of Get Req");
            barcode = Request.QueryString.Value;
            barcode = barcode.Substring(1);
            List<Ingredient> ingredients;
            FoodItem foodItem;
            NutritionLabel label;


            _foodDB = new FoodDBOperations(_foodServiceGateway);

            



            try
            {
                Console.WriteLine("GET " + barcode);

                
                foodItem = await _foodDB.GetScannedItemAsync(barcode);
                ingredients = await _foodDB.GetIngredientsListAsync(barcode);
                label = await _foodDB.GetNutritionLabelAsync(barcode);

                string jsonStr = "{";
                string foodItemStr = foodItem.FormatJsonString();
                string labelStr = label.FormatJsonString();
                string ingredientsStr = FormatIngredientsJsonString(ingredients);

                jsonStr += foodItemStr + ", " + labelStr + ", " + ingredientsStr + "}";



                return jsonStr;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "BIG OL FAIL";
            }

            //return await _foodDB.GetScannedItemAsync(barcode); unsure if we will need this later
        }

        [HttpPost]
        public async void Post()
        {
 
        }

        /// <summary>
        /// MAY NEED TO MOVE TO A HELPER CLASS
        /// </summary>
        /// <param name="ingredientList"></param>
        /// <returns></returns>
        public string FormatIngredientsJsonString(List<Ingredient> ingredientList)
        {
            string strNameList = "\"IngredientName\": [";
            string strAltList = "\"IngredientAlternateName\": [";
            string strDescList = "\"IngredientDescription\": [";

            for (int i = 0; i < ingredientList.Count; i++)
            {
                strNameList += $"\"{ingredientList[i].IngredientName}\"";
                strAltList += $"\"{ingredientList[i].AlternateName}\"";
                strDescList += $"\"{ingredientList[i].IngredientDescription}\"";

                if (i < ingredientList.Count - 1)
                {
                    strNameList += ",";
                    strAltList += ",";
                    strDescList += ",";
                }
                else if (i == ingredientList.Count - 1)
                {
                    strNameList += "]";
                    strAltList += "]";
                    strDescList += "]";
                }
            }

            return strNameList + ", " + strAltList + ", " + strDescList;
        }
    }
}
