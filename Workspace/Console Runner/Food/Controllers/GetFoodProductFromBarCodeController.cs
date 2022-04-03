using Console_Runner.FoodService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Food.Controllers

{
    
    [Route("api/[controller]")]
    [ApiController]
    public class GetFoodProductFromBarCodeController : ControllerBase
    {
        private readonly IFoodGateway _foodServiceGateway = new EFFoodGateway();
        private  FoodDBOperations _foodDB;
        private string barcode;

        [HttpPost]
        public async void Post()
        {
            _foodDB = new FoodDBOperations(_foodServiceGateway);

            //IFormCollection formData = Request.Form;

            //barcode = formData["barcode"];
            Console.WriteLine(barcode);
            Console.WriteLine(barcode);
            Console.WriteLine(barcode);
            Console.WriteLine(barcode);
            Console.WriteLine(barcode);
            //barcode = Request.Form["barcode"];
        }


        [HttpGet]
        public async Task<ActionResult<String>> GET()
        {
            List<Ingredient> ingredients;
            FoodItem foodItem;
            NutritionLabel label;

            barcode = Request.Form["barcode"];
            Console.WriteLine(barcode);

            //TODO: ADD CHECK TO SEE IF BARCODE IS IN OUR DB

            _foodDB = new FoodDBOperations(_foodServiceGateway);

            ingredients = await _foodDB.GetIngredientsListAsync(barcode);
            foodItem = await _foodDB.GetScannedItemAsync(barcode);
            label = await _foodDB.GetNutritionLabelAsync(barcode);
            
            string jsonStr = "{";
            string foodItemStr = foodItem.FormatJsonString();
            string labelStr = label.FormatJsonString();
            string ingredientsStr = FormatIngredientsJsonString(ingredients);

            jsonStr += foodItemStr + ", " + labelStr + ", " + ingredientsStr + "}";

            

            return jsonStr;
            //return await _foodDB.GetScannedItemAsync(barcode); unsure if we will need this later
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
