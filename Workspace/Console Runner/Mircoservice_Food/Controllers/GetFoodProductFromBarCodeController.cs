﻿using Console_Runner.AccountService;
using Console_Runner.FoodService;
using Console_Runner.Logging;
using Microsoft.AspNetCore.Mvc;
using Mircoservice_Food;
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
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private AccountDBOperations _accountDBOperations;
        private string barcode;
        private List<Ingredient> flaggedIngredients = new();


        [HttpGet]
        public async Task<ActionResult<string>> GET()
        {

            barcode = Request.QueryString.Value;
            barcode = barcode.Substring(1);
            List<Ingredient> ingredients = new();
            FoodItem foodItem;
            NutritionLabel label;


            _foodDB = new FoodDBOperations(_foodServiceGateway);
            _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;



            try
            {
                Console.WriteLine("GET " + barcode);

                
                foodItem = await _foodDB.GetScannedItemAsync(barcode, logger);

                ingredients = await _foodDB.GetIngredientsListAsync(barcode, logger);

                int userID = 0; //TODO NEED THE ACTUAL USER ID;
                List<FoodFlag> flags = await _accountDBOperations.GetAllAccountFlagsAsync(userID, logger);
               
                for( int i = 0; i < flags.Count; i++)
                {
                    for(int j = 0; j < ingredients.Count; j++)
                    {
                        if(flags[i].IngredientID == ingredients[j].IngredientID)
                        {
                            flaggedIngredients.Add(ingredients[j]);
                            Console.WriteLine(ingredients[j].IngredientName);
                        }
                    }
                }
                
                label = await _foodDB.GetNutritionLabelAsync(barcode, logger);
                List<(Nutrient, float)> nutrientListTuple = await _foodDB.GetNutrientListForUserDisplayAsync(barcode, logger);
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
                string ingredientsStr = FoodProductHelper.FormatIngredientsJsonString(ingredients, flaggedIngredients);

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