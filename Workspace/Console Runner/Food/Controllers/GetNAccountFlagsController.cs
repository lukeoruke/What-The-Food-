using Console_Runner.AccountService;
using Console_Runner.FoodService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Console_Runner.Logging;

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
        [HttpGet]
        public async Task<ActionResult<string>> GET()
        {
            int userID = 0;//TODO GET USER ID
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            FoodDBOperations _foodDBOperations = new FoodDBOperations(_foodGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;
            string page = Request.QueryString.Value;
            page = page.Substring(1);
            int numberOfItemsDisplayedAtOnce = 2;
            try
            {
                var allFlags = await _accountDBOperations.GetNAccountFlagsAsync(userID, numberOfItemsDisplayedAtOnce * int.Parse(page)
                    , numberOfItemsDisplayedAtOnce, logger);
                List<Ingredient> ingredients = new List<Ingredient>();
                for(int i = 0; i < allFlags.Count; i++)
                {
                    ingredients.Add(await _foodDBOperations.GetIngredient(allFlags[i].IngredientID, logger));
                }


                string jsonStr = "{";
                jsonStr += FormatIngredientsJsonString(ingredients);

                return jsonStr + "}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "Something went wrong getting the ingredients from flag list";
            }
        }
        public string FormatIngredientsJsonString(List<Ingredient> ingredientList)
        {
            string strNameList = "\"IngredientName\": [";
            string strAltList = "\"IngredientAlternateName\": [";
            string strDescList = "\"IngredientDescription\": [";
            string strIngIDList = "\"IngredientID\": [";
            for (int i = 0; i < ingredientList.Count; i++)
            {

                strNameList += $"\"{ingredientList[i].IngredientName}\"";
                strAltList += $"\"{ingredientList[i].AlternateName}\"";
                strDescList += $"\"{ingredientList[i].IngredientDescription}\"";
                strIngIDList += $"\"{ingredientList[i].IngredientID}\"";

                if (i < ingredientList.Count - 1)
                {
                    strNameList += ",";
                    strAltList += ",";
                    strDescList += ",";
                    strIngIDList += ",";
                }
                else if (i == ingredientList.Count - 1)
                {
                    strNameList += "]";
                    strAltList += "]";
                    strDescList += "]";
                    strIngIDList += "]";
                }
            }

            return strNameList + ", " + strIngIDList;
        }
    }
}





