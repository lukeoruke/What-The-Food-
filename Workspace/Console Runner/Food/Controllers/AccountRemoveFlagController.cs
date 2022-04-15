﻿using Console_Runner.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Food.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountRemoveFlagController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        [HttpPost]
        public async void Post()
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway);
            int userId = 0;// NEED TO GET USER ID
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();

                var ingsId = body.Split(",");
                if (ingsId[0] == "" || ingsId[0] == null)
                {
                    return;
                }

                for (int i = 0; i < ingsId.Length; i++)
                {
                    Console.WriteLine(ingsId[i]);
                    await _accountDBOperations.RemoveFoodFlagAsync(userId, int.Parse(ingsId[i]));
                }
            }
        }


    }
}




