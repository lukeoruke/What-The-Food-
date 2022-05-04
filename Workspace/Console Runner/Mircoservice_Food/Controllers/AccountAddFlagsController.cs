﻿using Console_Runner.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Console_Runner.Logging;

namespace Food.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountAddFlagsController : ControllerBase
    {
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        private readonly IActiveSessionTrackerGateway _EFActiveSessionTrackerGateway = new EFActiveSessionTrackerGateway();
        private int userId;


        [HttpPost]

        public async void Post()
        {
            AccountDBOperations _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amRGateway, _EFActiveSessionTrackerGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;
            try
            {
                string input = Request.QueryString.Value;
                string[] inputarr = input.Split('?');
                string JWT = inputarr[1];
                userId = await _accountDBOperations.getActiveUserAsync(JWT);
            }
            catch
            {
                Console.WriteLine("A problem occured in the accountAddFlagController while attempting to get the active user");
            }
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                
                var ingsId = body.Split(",");
                if (ingsId[0] == "" || ingsId[0] == null)
                {
                    return;
                }

                for(int i = 0; i < ingsId.Length; i++)
                {
                    await _accountDBOperations.AddFlagToAccountAsync(userId, int.Parse(ingsId[i]), logger);
                }
            }
        }


    }
}



