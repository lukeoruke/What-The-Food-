using Microsoft.AspNetCore.Mvc;
using Console_Runner.FoodService;
using Console_Runner.Logging;
using Mircoservice_Food;

namespace Food.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FlagSearchIngredientsController : ControllerBase
    {
        private const string UM_CATEGORY = "Data Store";

        private readonly IFoodGateway _foodGateway = new EFFoodGateway();
        private readonly IFoodUpdateGateway _foodUpdateGateway = new EFFoodUpdateGateway();

        [HttpGet]
        public async Task<ActionResult<string>> GET(string page,  string search)
        {
            FoodDBOperations _foodDBOperations = new FoodDBOperations(_foodGateway, _foodUpdateGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it

            logger.DefaultTimeOut = 5000;


            try
            {

                int numberOfItemsDisplayedAtOnce = 2;
                Console.WriteLine("GET " + search);
                var allIngredientList = await _foodDBOperations.GetIngredientBySearchAsync(search, numberOfItemsDisplayedAtOnce * int.Parse(page)
                    , numberOfItemsDisplayedAtOnce, logger);
                Console.WriteLine("Length of ing list(search function) = " + allIngredientList.Count());
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



