using Console_Runner.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

using Console_Runner.Logging;
using Microsoft.AspNetCore.Cors;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddAMRController : Controller
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();

        [EnableCors]

        [HttpPost]
        public async void Post()
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserEmail = "placeholder";
            logger.DefaultTimeOut = 5000;
            int userId = 0;// NEED TO GET USER ID
            
            
            //Request formData from the JS file
            IFormCollection formData = Request.Form;

            //Displays
            Console.WriteLine(formData["gender"]);
            Console.WriteLine(formData["weight"]);
            Console.WriteLine(formData["height"]);
            Console.WriteLine(formData["age"]);
            Console.WriteLine(formData["activity"]);

            //Converts the data variable to a either an int or float
            int amrWeight = Convert.ToInt32(formData["weight"]);
            float amrHeight = float.Parse(formData["height"], System.Globalization.CultureInfo.InvariantCulture);
            int amrAge = Convert.ToInt32(formData["age"]);


            //switch cases to check what activity was selected and then declares
            ActivityLevel userActivity = ActivityLevel.None;
            switch (formData["activity"])
            {
                case "None":
                    userActivity = ActivityLevel.None;
                    break;
                case "Light":
                    userActivity = ActivityLevel.Light;
                    break;
                case "Moderate":
                    userActivity = ActivityLevel.Moderate;
                    break;
                case "Daily":
                    userActivity = ActivityLevel.Daily;
                    break;
                case "Heavy":
                    userActivity = ActivityLevel.Heavy;
                    break;
            }


            var AMR = _accountDBOperations.AddAMRAsync(userId, formData["gender"] == "Male", amrWeight , amrHeight, amrAge, userActivity); ;

            


        }
    }
}