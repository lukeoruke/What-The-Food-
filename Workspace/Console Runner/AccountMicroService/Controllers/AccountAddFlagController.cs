namespace AccountMicroservice.Controllers
{
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Console_Runner.AccountService;
    namespace Microservice.AccountLogin.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AccountAddFlagController : ControllerBase
        {
            private const string UM_CATEGORY = "Data Store";
            private readonly IAccountGateway _accountAccess = new EFAccountGateway();
            private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
            private readonly IFlagGateway _flagGateway = new EFFlagGateway();

            [HttpPost]
            public async void Post()
            {
                AccountDBOperations _accountDBOperations = new AccountDBOperations
                (_accountAccess, _permissionService, _flagGateway);
                //IFormCollection formData = Request.Form;
            }
        }
    }
}
