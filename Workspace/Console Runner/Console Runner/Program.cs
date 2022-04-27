// See https://aka.ms/new-console-template for more information
/*
 * CURRENTLY ONLY A DEMO FILE
 */
using Microsoft.Extensions.DependencyInjection;
using Console_Runner;
using Console_Runner.Logging;
using Console_Runner.FoodService;
using Console_Runner.AccountService;

[STAThread]
async static void Main()
{
}

AccountDBOperations accdbops = new(new EFAccountGateway(), new EFAuthorizationGateway(), new EFFlagGateway());

await accdbops.UserSignUpAsync(new Account()
{
    Email = "email",
    Password ="password",
    FName = "a",
    LName = "b",
    NewsBias = 1,
    IsActive = true,
    Enabled = true,
    salt = "what"
});

/*Console.WriteLine("Program running...");
FoodDBOperations foodDBOperations = new FoodDBOperations(new EFFoodGateway());
FoodItem food1 = new FoodItem("barcode", "monster", "who what", "link to pic");
FoodItem food2 = new FoodItem("anotherbarcode", "choccy milk", "who what", "link to pic");
FoodItem food3 = new FoodItem("athirdbarcode", "tea", "some dude", "link to pic");
foodDBOperations.AddFoodItemAsync(food1).Wait();
foodDBOperations.AddFoodItemAsync(food2).Wait();
foodDBOperations.AddFoodItemAsync(food3).Wait();*/


/*
Console.WriteLine("Program running...");
LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
await logger.LogAsync("test", LogLevel.Info, Category.Business, DateTime.UtcNow, "this is a test message");
await logger.LogAsync("test", LogLevel.Info, Category.Business, DateTime.UtcNow, "this is a second test message");
 */
/**
IDataAccess dal = new DummyDaL();
ILogger log = new Logging(dal);
UM um = new UM(dal, log); //UM has been removed from this scope
**/