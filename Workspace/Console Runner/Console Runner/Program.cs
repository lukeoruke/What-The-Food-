// See https://aka.ms/new-console-template for more information
/*
 * CURRENTLY ONLY A DEMO FILE
 */
using Microsoft.Extensions.DependencyInjection;
using Console_Runner;
using Console_Runner.Logging;
using Console_Runner.FoodService;


[STAThread]
async static void Main()
{
}


/*Console.WriteLine("Program running...");
LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
logger.DefaultTimeOut = 5000;
logger.UserID = "user";
FoodDBOperations foodDBOperations = new FoodDBOperations(new EFFoodGateway(), new EFFoodUpdateGateway());
FoodItem food1 = new FoodItem("barcode", "monster", "who what", "link to pic");
FoodItem food2 = new FoodItem("anotherbarcode", "choccy milk", "who what", "link to pic");
FoodItem food3 = new FoodItem("athirdbarcode", "tea", "some dude", "link to pic");
foodDBOperations.AddFoodItemAsync(food1, logger).Wait();
foodDBOperations.AddFoodItemAsync(food2, logger).Wait();
foodDBOperations.AddFoodItemAsync(food3, logger).Wait();
await foodDBOperations.AddFoodUpdateAsync(new FoodIngredientChange(food1, DateTime.Now, "ing change for monster", new[] { new Ingredient("some ing", "whatt", "iunno") }, new[] { new Ingredient("removed ign", "????", "bad word") }), logger);
await foodDBOperations.AddFoodUpdateAsync(new FoodRecall(food1, DateTime.Now, "sermonella", new[] { "location" }, new[] { 10 }, new[] { DateTime.Parse("2020-01-03") }), logger);
await foodDBOperations.AddFoodUpdateAsync(new FoodRecall(food2, DateTime.Now, "norovirus!", new[] { "otherlocation" }, new int[0], new[] { DateTime.Parse("2020-01-23") }), logger);
*/

Console.WriteLine("Program running...");
LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
logger.DefaultTimeOut = 5000;
logger.UserID = "user";
FoodDBOperations foodDBOperations = new FoodDBOperations(new EFFoodGateway(), new EFFoodUpdateGateway());
FoodItem food1 = await foodDBOperations.GetScannedItemAsync("034000245659", logger);
List<Ingredient> ingredients1 = await foodDBOperations.GetIngredientsListAsync(food1.Barcode);
FoodItem food2 = await foodDBOperations.GetScannedItemAsync("008077102207", logger);
List <Ingredient> ingredients2 = await foodDBOperations.GetIngredientsListAsync(food2.Barcode);

FoodIngredientChange update1 = new FoodIngredientChange(food1,
                                                        DateTime.Now,
                                                        "Test Ingredient Change",
                                                        ingredients1.Take(2),
                                                        new Ingredient[0]);

FoodRecall update2 = new FoodRecall(food1,
                                    DateTime.Now,
                                    "Salmonella",
                                    new string[0],
                                    new int[0],
                                    new[] { DateTime.Parse("2020-01-03") });

FoodRecall update3 = new FoodRecall(food2,
                                    DateTime.Now,
                                    "norovirus!",
                                    new[] { "otherlocation" },
                                    new[] { 4823334 },
                                    new[] { DateTime.Parse("2020-01-23") });

await foodDBOperations.AddFoodUpdateAsync(update1, logger);
await foodDBOperations.AddFoodUpdateAsync(update2, logger);
await foodDBOperations.AddFoodUpdateAsync(update3, logger);


/*Console.WriteLine("Program running...");
LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
await logger.LogAsync("test", LogLevel.Info, Category.Business, DateTime.UtcNow, "this is a test message");
await logger.LogAsync("test", LogLevel.Info, Category.Business, DateTime.UtcNow, "this is a second test message");*/

/**
IDataAccess dal = new DummyDaL();
ILogger log = new Logging(dal);
UM um = new UM(dal, log); //UM has been removed from this scope
**/