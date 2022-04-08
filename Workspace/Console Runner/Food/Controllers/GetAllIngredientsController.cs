﻿
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Console_Runner.AccountService;
    using Console_Runner.FoodService;
    namespace Food.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class GetAllIngredientsController : ControllerBase
        {
            private const string UM_CATEGORY = "Data Store";
            private readonly IAccountGateway _accountAccess = new EFAccountGateway();
            private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
            private readonly IFlagGateway _flagGateway = new EFFlagGateway();
            private readonly IFoodGateway _foodGateway = new EFFoodGateway();
            [HttpGet]
            public async Task<ActionResult<string>> GET()
            {
                AccountDBOperations _accountDBOperations = new AccountDBOperations
                (_accountAccess, _permissionService, _flagGateway);
                FoodDBOperations _foodDBOperations = new FoodDBOperations(_foodGateway);
                
                try{
                var allIngredientList = await _foodDBOperations.getAllIngredientsAsync();
                Console.WriteLine("Length of ing list = " + allIngredientList.Count());
                string jsonStr = "{";
                jsonStr += FormatIngredientsJsonString(allIngredientList);
                Console.WriteLine(jsonStr);
                return jsonStr + "}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return "Something went wrong getting the ingredients list to display on food flags page";
                }


        }
            public string FormatIngredientsJsonString(List<Ingredient> ingredientList)
            {
                string strNameList = "\"IngredientName\": [";
                string strAltList = "\"IngredientAlternateName\": [";
                string strDescList = "\"IngredientDescription\": [";

                for (int i = 0; i < ingredientList.Count; i++)
                {

                    strNameList += $"\"{ingredientList[i].IngredientName}\"";
                    strAltList += $"\"{ingredientList[i].AlternateName}\"";
                    strDescList += $"\"{ingredientList[i].IngredientDescription}\"";

                    if (i < ingredientList.Count - 1)
                    {
                        strNameList += ",";
                        strAltList += ",";
                        strDescList += ",";
                    }
                    else if (i == ingredientList.Count - 1)
                    {
                        strNameList += "]";
                        strAltList += "]";
                        strDescList += "]";
                    }
                }

                return strNameList;
            }
        }
    }
