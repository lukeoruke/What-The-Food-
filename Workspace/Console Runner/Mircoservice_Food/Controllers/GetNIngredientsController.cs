using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Console_Runner.Logging;
using Console_Runner.FoodService;
using Mircoservice_Food;

namespace Food.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class GetNIngredientsController : ControllerBase
        {
            private const string UM_CATEGORY = "Data Store";

            private readonly IFoodGateway _foodGateway = new EFFoodGateway();
            private readonly IFoodUpdateGateway _foodUpdateGateway = new EFFoodUpdateGateway();
            [HttpGet]
            public async Task<ActionResult<string>> GET()
            {
                FoodDBOperations _foodDBOperations = new FoodDBOperations(_foodGateway, _foodUpdateGateway);
                LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
                // TODO: replace this string with the user email when we can get it
                logger.UserID = "placeholder";
                logger.DefaultTimeOut = 5000;
                string page = Request.QueryString.Value;
                page = page.Substring(1);
                try
                {
                    Console.WriteLine("Page " + page);
                    var allIngredientList = await _foodDBOperations.GetNIngredientsAsync(5*int.Parse(page), 5, logger);
                    Console.WriteLine("Length of ing list = " + allIngredientList.Count());
                    string jsonStr = "{";
                    jsonStr += FoodFlagsHelper.FormatIngredientsJsonString(allIngredientList);
                    Console.WriteLine(jsonStr);
                    return jsonStr + "}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return "Something went wrong getting the ingredients list to display on food flags page";
                }
            }
        }
    }
