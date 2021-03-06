using Console_Runner.AccountService;
using Console_Runner.AccountService.Authentication;
using Console_Runner.FoodService;
using Console_Runner.Logging;
using Microservice_Food;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Mircoservice_Food;
using System.Text.Json;

namespace Food.Controllers

{
    
    [Route("api/[controller]")]
    [ApiController]
    public class GetFoodProductFromBarCodeController : ControllerBase
    {
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IAuthenticationService _JWTAuthenticationService = new JWTAuthenticationService("TESTDATAHERE");
        private ScanHelper FDC = new ScanHelper();
        private const string UM_CATEGORY = "Data Store";
        private readonly IFoodGateway _foodServiceGateway = new EFFoodGateway();
        private readonly IFoodUpdateGateway _foodUpdateGateway = new EFFoodUpdateGateway();
        private  FoodDBOperations _foodDB;
        private IFormCollection formData;
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private AccountDBOperations _accountDBOperations;
        private string barcode;
        private List<Ingredient> flaggedIngredients = new();
        private readonly IAMRGateway _amrGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        int userID = -1;
        /// <summary>
        /// HttpGet request for recieving a food product from a barcode
        /// </summary>
        /// <returns>a string formatted as a Json object</returns>
        ///
        [EnableCors]
        [HttpGet]
        public async Task<ActionResult<string>> GET(string barcode, string token)
        {
            Console.WriteLine("Recieved");

            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            userID = await _accountDBOperations.GetActiveUserAsync(token);

            //creation of foodDB objs
            List<Ingredient> ingredients = new();
            FoodItem foodItem;
            NutritionLabel label;
            _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amrGateway, _EFActiveSessionTrackerGateway);
            _foodDB = new FoodDBOperations(_foodServiceGateway, _foodUpdateGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserEmail = "Unknown";
            logger.DefaultTimeOut = 5000;

            try
            {
                Console.WriteLine("GET " + barcode);

                //try to get the food item from our own DB
                foodItem = await _foodDB.GetScannedItemAsync(barcode, logger);

                if (foodItem == null)    //if the food item doesn't exist in our DB, attempt to add it to the DB
                {
                    int response = await FDC.SearchAndAdd(barcode); //call to ScanHelper.cs
                    //Console.WriteLine("Returning get from wrapper " + response);

                    if (response == 1)
                    {
                        foodItem = await _foodDB.GetScannedItemAsync(barcode);
                        Console.WriteLine("1");
                        if (foodItem == null)
                        {
                            _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Info, Category.Business, DateTime.Now,
                                                           $"User scanned a previously unscanned barcode {barcode}; corresponding food item added to database.");
                            return "No Corresponding UPC";
                        }
                    }
                    else if (response == 0)
                    {
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Debug, Category.Business, DateTime.Now,
                                                       $"User scanned a previously unscanned barcode {barcode}; search found no items.");
                        return "Invalid Input";
                    }
                    else if (response == -1)
                    {
                        _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Warning, Category.Business, DateTime.Now,
                                                       $"User scanned a previously unscanned barcode {barcode}; error occurred with scan.");
                        return "An Error With The Scan Has Occured";
                    }
                }

                //get list of ingredients and check for user flags
                ingredients = await _foodDB.GetIngredientsListAsync(barcode, logger);

                List<FoodFlag> flags = await _accountDBOperations.GetAllAccountFlagsAsync(userID, logger);
               
                for( int i = 0; i < flags.Count; i++)
                {
                    for(int j = 0; j < ingredients.Count; j++)
                    {
                        if(flags[i].IngredientID == ingredients[j].IngredientID)
                        {
                            flaggedIngredients.Add(ingredients[j]);
                        }
                    }
                }

                //Fetch information from the DB of a given barcode
                label = await _foodDB.GetNutritionLabelAsync(barcode, logger);
                List<(Nutrient, float)> nutrientListTuple = await _foodDB.GetNutrientListForUserDisplayAsync(barcode, logger);
                List<Nutrient> nutrientList = new();
                for (int i = 0; i < nutrientListTuple.Count; i++)
                {
                    label.AddNutrient(nutrientListTuple[i]);
                    nutrientList.Add(nutrientListTuple[i].Item1);
                }

                //Begin formatting Json string response
                string jsonStr = "{";
                string foodItemStr = foodItem.FormatJsonString();
                
                //nutrientList = _foodDB.get
                string labelStr = label.FormatJsonString();
                string ingredientsStr = FoodProductHelper.FormatIngredientsJsonString(ingredients, flaggedIngredients);

                jsonStr += foodItemStr + ", " + labelStr + ", " + ingredientsStr + "}";



                _ = logger.LogWithSetUserAsync(Console_Runner.Logging.LogLevel.Info, Category.Business, DateTime.Now,
                                               $"User scanned a previously scanned barcode {barcode}.");
                return jsonStr;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "BIG OL FAIL";
            }

            //return await _foodDB.GetScannedItemAsync(barcode); unsure if we will need this later
        }
    }
}
