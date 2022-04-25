using Console_Runner.FoodService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Food.Executables;

namespace Food.Controllers

{
    
    [Route("api/[controller]")]
    [ApiController]
    public class GetFoodProductFromBarCodeController : ControllerBase
    {
        private ScanHelper FDC = new ScanHelper();
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
            List<Ingredient> ingredients = new();
            FoodItem foodItem;
            NutritionLabel label;

            _foodDB = new FoodDBOperations(_foodServiceGateway);

            try
            {
                Console.WriteLine("GET " + barcode);


                foodItem = null; //await _foodDB.GetScannedItemAsync(barcode);
                if(foodItem == null)    //if the food item doesn't exist in our DB, add it to the DB
                {
                    var response = await FDC.SearchAndAdd(barcode);
                    Console.WriteLine("Returning get from wrapper " + response);
                }
                
                //Fetch information from the DB of a given barcode

                ingredients = await _foodDB.GetIngredientsListAsync(barcode);
                label = await _foodDB.GetNutritionLabelAsync(barcode);
                List<(Nutrient, float)> nutrientListTuple = await _foodDB.GetNutrientListForUserDisplay(barcode);
                List<Nutrient> nutrientList = new();
                for (int i = 0; i < nutrientListTuple.Count; i++)
                {
                    label.AddNutrient(nutrientListTuple[i]);
                    nutrientList.Add(nutrientListTuple[i].Item1);
                }

                string jsonStr = "{";
                string foodItemStr = foodItem.FormatJsonString();
                
                //nutrientList = _foodDB.get
                string labelStr = label.FormatJsonString();
                string ingredientsStr = FDC.FormatIngredientsJsonString(ingredients);

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
    }
}
