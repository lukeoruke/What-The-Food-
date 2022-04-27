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
        private readonly IFoodUpdateGateway _foodUpdateGateway = new EFFoodUpdateGateway();
        private FoodDBOperations _foodDB;
        private IFormCollection formData;
        private string barcode;
      
        /// <summary>
        /// HttpGet request for recieving a food product from a barcode
        /// </summary>
        /// <returns>a string formatted as a Json object</returns>
        [HttpGet]
        public async Task<ActionResult<string>> GET()
        {
<<<<<<< HEAD
=======
            Console.WriteLine("This is the start of Get Req");
            //get request info and format it
>>>>>>> News-Branch-Final
            barcode = Request.QueryString.Value;
            barcode = barcode.Substring(1);


            //creation of foodDB objs
            List<Ingredient> ingredients = new(); //ASK MATT ABOUT THIS
            FoodItem foodItem;
            NutritionLabel label;

<<<<<<< HEAD

            _foodDB = new FoodDBOperations(_foodServiceGateway, _foodUpdateGateway);
            _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;

            try
            {
                foodItem = await _foodDB.GetScannedItemAsync(barcode, logger);
                ingredients = await _foodDB.GetIngredientsListAsync(barcode, logger);

                int userID = 0; //TODO NEED THE ACTUAL USER ID;
                List<FoodFlag> flags = await _accountDBOperations.GetAllAccountFlagsAsync(userID, logger);
               
                for( int i = 0; i < flags.Count; i++)
=======
            _foodDB = new FoodDBOperations(_foodServiceGateway); //dependency injection

            try
            {
                Console.WriteLine("GET " + barcode);

                //try to get the food item from our own DB
                foodItem = await _foodDB.GetScannedItemAsync(barcode);

                if(foodItem == null)    //if the food item doesn't exist in our DB, attempt to add it to the DB
>>>>>>> News-Branch-Final
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
<<<<<<< HEAD
 
        }

        /// <summary>
        /// MAY NEED TO MOVE TO A HELPER CLASS
        /// </summary>
        /// <param name="ingredientList"></param>
        /// <returns></returns>
        private string FormatIngredientsJsonString(List<Ingredient> ingredientList)
        {
            string strNameList = "\"IngredientName\": [";
            string strAltList = "\"IngredientAlternateName\": [";
            string strDescList = "\"IngredientDescription\": [";
            string flaggedItemList = "\"FlaggedItems\": [";
            for(int i = 0; i < flaggedIngredients.Count; i++)
            {
                flaggedItemList += $"\"{flaggedIngredients[i].IngredientName}\"";
                if (i < flaggedIngredients.Count - 1)
                {
                    flaggedItemList += ",";
                }
                else if (i == flaggedIngredients.Count - 1)
                {
                    flaggedItemList += "]";
                }

            }

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
            if (flaggedIngredients.Count == 0)
            {
                return strNameList + ", " + strAltList + ", " + strDescList;
            }
            else
            {
                return strNameList + ", " + strAltList + ", " + strDescList + ", " + flaggedItemList;
            }
            
=======
>>>>>>> News-Branch-Final
        }
    }
}
