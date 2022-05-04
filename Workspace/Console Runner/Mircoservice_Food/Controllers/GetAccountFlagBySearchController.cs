﻿using Microsoft.AspNetCore.Mvc;
using Console_Runner.AccountService;
using Console_Runner.FoodService;
using Console_Runner.Logging;
using Mircoservice_Food;

namespace Food.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GetAccountFlagBySearchController : ControllerBase
    {
        private const string UM_CATEGORY = "Data Store";

        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IFoodGateway _foodGateway = new EFFoodGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IFoodUpdateGateway _foodUpdateGateway = new EFFoodUpdateGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        [HttpGet]
        public async Task<ActionResult<string>> GET()
        {
            int userID = 0;//TODO GET USER ID
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            FoodDBOperations _foodDBOperations = new FoodDBOperations(_foodGateway, _foodUpdateGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;
            string input = Request.QueryString.Value;
            string[] inputarr = input.Split('?');
            string search = inputarr[1];

            string page = inputarr[2];
            int numberOfItemsDisplayedAtOnce = 1;
            try
            {
                var allFlags =  await _accountDBOperations.GetAllAccountFlagsAsync(userID, logger);
                List<Ingredient> ingredients = new List<Ingredient>();
                for(int i = 0; i < allFlags.Count; i++)
                {
                    ingredients.Add(await _foodDBOperations.GetIngredient(allFlags[i].IngredientID, logger));
                }

                ingredients = ingredients.Where(x => x.IngredientName.Contains(search)).OrderBy(x => x.IngredientName).Skip(numberOfItemsDisplayedAtOnce * int.Parse(page)).Take(numberOfItemsDisplayedAtOnce).ToList();

                
                Console.WriteLine("(SearchFlags)Length of ing list = " + ingredients.Count());

                string jsonStr = "{";
                jsonStr += FoodFlagsHelper.FormatIngredientsJsonString(ingredients);
                Console.WriteLine(jsonStr);
                return jsonStr + "}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "(GetAccountFlagBySearchController)-Something went wrong getting the ingredients from flag list";
            }
        }
    }
}





