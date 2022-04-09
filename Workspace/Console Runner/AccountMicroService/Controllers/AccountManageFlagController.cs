using Console_Runner.AccountService;
using Microsoft.AspNetCore.Mvc;

namespace AccountMicroservice.Controllers
{
    public class AccountManageFlagController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        [HttpPost]
        public async void Post()
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations (_accountAccess, _permissionService, _flagGateway);
            IFormCollection formData = Request.Form;

            await _accountDBOperations.AddFlagToAccountAsync(11, 11);
        }


    }
}
