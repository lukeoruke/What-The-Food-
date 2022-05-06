using Console_Runner.AccountService;
using Console_Runner.FoodService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Console_Runner.Logging;
using Mircoservice_Food;

namespace Food.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GetNAccountFlagsController : ControllerBase
    {
        private const string UM_CATEGORY = "Data Store";

        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IFoodGateway _foodGateway = new EFFoodGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private readonly IFoodUpdateGateway _foodUpdateGateway = new EFFoodUpdateGateway();
        [HttpGet]
        public async Task<ActionResult<string>> GET(string page, string token)
        {
            
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            FoodDBOperations _foodDBOperations = new FoodDBOperations(_foodGateway, _foodUpdateGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;
            int userId = -1;
            string rToken = token.Split("\"")[1];
            userId = await _accountDBOperations.getActiveUserAsync(rToken);
            int numberOfItemsDisplayedAtOnce = 2;
            try
            {
                var allFlags = await _accountDBOperations.GetNAccountFlagsAsync(userId, numberOfItemsDisplayedAtOnce * int.Parse(page)
                    , numberOfItemsDisplayedAtOnce, logger);
                List<Ingredient> ingredients = new List<Ingredient>();
                for(int i = 0; i < allFlags.Count; i++)
                {
                    ingredients.Add(await _foodDBOperations.GetIngredient(allFlags[i].IngredientID, logger));
                }


                string jsonStr = "{";
                jsonStr += FoodFlagsHelper.FormatIngredientsJsonString(ingredients);

                return jsonStr + "}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "Something went wrong getting the ingredients from flag list";
            }
        }
    }
}





