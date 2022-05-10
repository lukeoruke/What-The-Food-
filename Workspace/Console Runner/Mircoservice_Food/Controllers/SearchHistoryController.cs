using Console_Runner.AccountService;
using Console_Runner.FoodService;
using Console_Runner.Logging;
using Microservice_Food;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Mircoservice_Food;
using System.Text.Json;

namespace Mircoservice_Food.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchHistoryController : ControllerBase
    {
        private ScanHelper FDC = new ScanHelper();
        private const string UM_CATEGORY = "Data Store";
        private readonly IFoodGateway _foodServiceGateway = new EFFoodGateway();
        private FoodDBOperations _foodDB;
        private IFormCollection formData;
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private AccountDBOperations _accountDBOperations;
        private string barcode;
        private List<Ingredient> flaggedIngredients = new();
        private readonly IAMRGateway _amrGateway = new EFAMRGateway();

        [EnableCors]
        [HttpGet]

        [HttpPost]
        public async void Post()
        {

        }
    }
}
