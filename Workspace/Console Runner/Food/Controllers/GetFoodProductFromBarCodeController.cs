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
      
        /// <summary>
        /// HttpGet request for recieving a food product from a barcode
        /// </summary>
        /// <returns>a string formatted as a Json object</returns>
        [HttpGet]
        public async Task<ActionResult<string>> GET()
        {
            Console.WriteLine("This is the start of Get Req");
            //get request info and format it
            barcode = Request.QueryString.Value;
            barcode = barcode.Substring(1);


            //creation of foodDB objs
            List<Ingredient> ingredients = new(); //ASK MATT ABOUT THIS
            FoodItem foodItem;
            NutritionLabel label;

            _foodDB = new FoodDBOperations(_foodServiceGateway); //dependency injection

            try
            {
                Console.WriteLine("GET " + barcode);

                //try to get the food item from our own DB
                foodItem = await _foodDB.GetScannedItemAsync(barcode);

                if(foodItem == null)    //if the food item doesn't exist in our DB, attempt to add it to the DB
                {
                    int response = await FDC.SearchAndAdd(barcode); //call to ScanHelper.cs
                    Console.WriteLine("Returning get from wrapper " + response);

                    if (response == 1)
                    {
                        foodItem = await _foodDB.GetScannedItemAsync(barcode);

                        if (foodItem == null)
                        {
                            return "No Corresponding UPC";
                        }
                    }
                    else if (response == 0)
                    {
                        return "Invalid Input";
                    }
                    else if (response == -1)
                    {
                        return "An Error With The Scan Has Occured";
                    }
                }
                
                //Fetch information from the DB of a given barcode
                ingredients = await _foodDB.GetIngredientsListAsync(barcode);
                label = await _foodDB.GetNutritionLabelAsync(barcode);
                List<(Nutrient, float)> nutrientListTuple = await _foodDB.GetNutrientListForUserDisplayAsync(barcode);
                List<Nutrient> nutrientList = new();
                for (int i = 0; i < nutrientListTuple.Count; i++)
                {
                    label.AddNutrient(nutrientListTuple[i]);
                    nutrientList.Add(nutrientListTuple[i].Item1);
                }

                //Begin formatting Json string response
                string jsonStr = "{";
                string foodItemStr = foodItem.FormatJsonString();
                
                //nutrientList = _foodDB.get ASK MATT ABOUT THIS
                string labelStr = label.FormatJsonString();
                string ingredientsStr = FDC.FormatIngredientsJsonString(ingredients);

                jsonStr += foodItemStr + ", " + labelStr + ", " + ingredientsStr + "}";

                return jsonStr;

            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "BIG OL FAIL";
            }
        }

        [HttpPost]
        public async void Post()
        {
        }
    }
}
