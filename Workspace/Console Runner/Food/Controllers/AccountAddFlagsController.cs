using Console_Runner.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Food.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountAddFlagsController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        [HttpPost]
        public async void Post()
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            int userId = 0;
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                var ingsId = body.Split(",");
                Console.WriteLine(ingsId[0]);
                for(int i = 0; i < ingsId.Length; i++)
                {
                    await _accountDBOperations.AddFlagToAccountAsync(userId, ingsId[i]);
                }
            }

            
            Console.WriteLine("TEST WAS ABOVE THIS");
            //await _accountDBOperations.AddFlagToAccountAsync(11, 11);
        }


    }
}



