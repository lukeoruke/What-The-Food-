using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<string>> GET(string page, string token, string search)
        {
            int userId = 0;//TODO GET USER ID
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            FoodDBOperations _foodDBOperations = new FoodDBOperations(_foodGateway, _foodUpdateGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserEmail = "placeholder";
            logger.DefaultTimeOut = 5000;
            userId = await _accountDBOperations.GetActiveUserAsync(token);

            if ((await _accountDBOperations.GetUserAccountAsync(userId)).CollectData)
            {
                logger.UserEmail = (await _accountDBOperations.GetUserAccountAsync(userId)).Email;
            }
            else
            {
                logger.UserEmail = null;
            }



            int numberOfItemsDisplayedAtOnce = 5;
            try
            {
                var allFlags =  await _accountDBOperations.GetAllAccountFlagsAsync(userId, logger);
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





