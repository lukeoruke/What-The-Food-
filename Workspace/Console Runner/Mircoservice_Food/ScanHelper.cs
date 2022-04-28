using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;
using Console_Runner.FoodService;
using Console_Runner.AccountService;
using Console_Runner.Logging;

namespace Microservice_Food
{
    public class ScanHelper
    {
        private readonly IAMRGateway _amRGateway = new EFAMRGateway();
        //dependency injection food DB
        private const string UM_CATEGORY = "Data Store";
        private readonly IFoodGateway _foodServiceGateway = new EFFoodGateway();
        private readonly IFoodUpdateGateway _foodUpdateGateway = new EFFoodUpdateGateway();
        private FoodDBOperations _foodDB;
        private IFormCollection formData;
        //dependency injection account and logging DB
        private readonly IAccountGateway _accountAccess = new EFAccountGateway();
        private readonly IAuthorizationGateway _permissionService = new EFAuthorizationGateway();
        private readonly IFlagGateway _flagGateway = new EFFlagGateway();
        private readonly IAMRGateway _amrGateway = new EFAMRGateway();
        private AccountDBOperations _accountDBOperations;

        private readonly HttpClient client;
        private readonly string url = "https://api.nal.usda.gov/fdc/";
        private readonly string apiKey = "lpdKjKHTtT3zCxQebMkJgGkvxgWqmuD9OL2NqviF";
        private NutrientValues nutValues = new NutrientValues();

        /// <summary>
        /// Initilization of Class, formatting HttpClient obj
        /// </summary>
        public ScanHelper()
        {
            client = new HttpClient();  //will ddos system, don't create a new instance for each obj. Use pooled instance. It is an implicit dependency
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add("apiKey", apiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Takes in a barcode that doesn't exist within our database and Queries FDC,
        /// returned values are then parsed and added to our DB
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns>A response whether the addition was successful or not.
        ///          1: Success
        ///          0: No items correspond to search
        ///         -1: Error has occured               
        /// </returns>
        public async Task<int> SearchAndAdd(string barcode)
        {
            _foodDB = new FoodDBOperations(_foodServiceGateway, _foodUpdateGateway);    //dependency injection
            _accountDBOperations = new AccountDBOperations(_accountAccess, _permissionService, _flagGateway, _amrGateway);
            LogService logger = LogServiceFactory.GetLogService(LogServiceFactory.DataStoreType.EntityFramework);
            // TODO: replace this string with the user email when we can get it
            logger.UserID = "placeholder";
            logger.DefaultTimeOut = 5000;

            try
            {
                FoodInfoJson queryResponse = await GET(barcode); //search barcode from FDC API

                if (queryResponse.totalHits < 1)
                {
                    return 0;
                }

                //new variable creation
                FoodItem newFood;
                NutritionLabel newLabel;
                foreach (Food food in queryResponse.foods)   //for every response we get we parse and add to our DB
                {
                    //Reset reused varaibles
                    List<Nutrient> newNutrients = new List<Nutrient>();
                    nutValues.ClearValues();

                    //make a new food object to be added to DB
                    if (food.subbrandName == null)
                        newFood = new FoodItem(food.gtinUpc, food.lowercaseDescription, food.brandName, "");
                    else
                        newFood = new FoodItem(food.gtinUpc, food.subbrandName, food.brandName, "");

                    //for this food item go through all nutrients and note what they are and what their values are
                    foreach (FoodNutrient nutrient in food.foodNutrients)
                    {
                        newNutrients.Add(new Nutrient(nutrient.nutrientName));
                        nutValues.AddNutrientValue(nutrient.nutrientName, (float)nutrient.value);
                    }

                    //create new nutritionLabel obj from values
                    newLabel = new NutritionLabel(nutValues.calories, 1, food.servingSize, nutValues.totalFat,
                                               nutValues.saturatedFat, nutValues.transFat, nutValues.cholestrol,
                                               nutValues.sodium, nutValues.carbs, nutValues.dietaryFiber, nutValues.totalSugars,
                                               0, nutValues.protein, nutValues.additionalNutrients, food.gtinUpc);

                    //create new List<Ingredients> obj from values
                    List<Ingredient> newIngredients = StringToIngredients(food.ingredients);
                    //Add new food, ingredients, and label to DB
                    await _foodDB.AddNewProductAsync(newFood, newLabel, newIngredients, logger);
                    /* 
                    Console.WriteLine(newFood.FormatJsonString());
                    Console.WriteLine(newLabel.FormatJsonString());
                    foreach (Ingredient i in newIngredients)
                    {
                        Console.WriteLine(i.FormatIngredientsJsonString());
                    } 
                    */
                }

                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// Takes in a string value (a barcode) and queries the FDC API, parses the response and returnes an obj 
        /// that holds the values of the returned foods
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        private async Task<FoodInfoJson> GET(string barcode)
        {
            try
            {
                //Console.WriteLine("Requesting!: " + client.BaseAddress + "v1/foods/search?query=" + barcode + "&api_key=" + apiKey);
                var streamTask = client.GetStreamAsync(client.BaseAddress + "v1/foods/search?query=" + barcode + "&api_key=" + apiKey);

                var values = await JsonSerializer.DeserializeAsync<FoodInfoJson>(await streamTask);
                //Console.WriteLine("printing values: " + values.foods[0].ToString());

                return values;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        /// <summary>
        /// Takes in a string of ingredients returned from the FDC API and parses them into individual Ingredient 
        /// objects
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A list of Ingredients from the parsed string</returns>
        public List<Ingredient> StringToIngredients(string str)
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            string temp = "";
            int openParentheses = 0;

            //get all ingredients from the string value
            while (str.Length > 0)   //this loop ensures that we properly get all food ingredients without cutting off too early
            {
                if (str[0] == '(' || str[0] == '[')
                {
                    openParentheses++;
                    temp += str[0];
                    str = str.Substring(1);
                }
                else if (str[0] == ')' || str[0] == ']')
                {
                    openParentheses--;
                    temp += str[0];
                    str = str.Substring(1);
                }
                else if (str[0] == ',' && openParentheses == 0)
                {
                    ingredients.Add(new Ingredient(temp.Trim(), "", ""));
                    str = str.Substring(1);
                    temp = "";
                }
                else
                {
                    temp += str[0];
                    str = str.Substring(1);
                }
            }

            ingredients.Add(new Ingredient(temp, "", "")); //one last value not added, added here

            return ingredients;
        }

        /// <summary>
        /// Formats a json file to a string, to be sent to front end
        /// </summary>
        /// <param name="ingredientList"></param>
        /// <returns>string value representing the parsed Json data</returns>
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

            return strNameList + ", " + strAltList + ", " + strDescList;
        }
    }


    /// <summary>
    /// A helper class that will denote the numerical values of a returned food product and store them for later
    /// </summary>
    public class NutrientValues
    {
        public int calories { get; set; }
        public int totalFat { get; set; }
        public int saturatedFat { get; set; }
        public int transFat { set; get; }
        public int cholestrol { set; get; }
        public int sodium { set; get; }
        public int carbs { set; get; }
        public int dietaryFiber { set; get; }
        public int totalSugars { set; get; }
        public int addedSugar { set; get; }
        public int protein { set; get; }
        public List<(Nutrient, float)> additionalNutrients { get; set; }    //Any aditional nutrients that are returned

        /// <summary>
        /// Base constructor, will initialize all values to 0, and create an empty list for additionalNutrients
        /// </summary>
        public NutrientValues()
        {
            calories = totalFat = saturatedFat = transFat = cholestrol = sodium =
                carbs = dietaryFiber = totalSugars = addedSugar = protein = 0;
            additionalNutrients = new List<(Nutrient, float)>();
        }

        /// <summary>
        /// Will clear all the held values back to 0, and empty list for additionalNutrients
        /// </summary>
        public void ClearValues()
        {
            calories = totalFat = saturatedFat = transFat = cholestrol = sodium =
                carbs = dietaryFiber = totalSugars = addedSugar = protein = 0;
            additionalNutrients = new List<(Nutrient, float)>();
        }

        /// <summary>
        /// Will take in a nutrient parsed from FDC API as a string and its amount value, and assign it to its appropriate
        /// value for the object. 
        /// </summary>
        /// <param name="nut"></param>
        /// <param name="val"></param>
        public void AddNutrientValue(string nut, float val)
        {
            if (nut.ToLower().Contains("energy"))
            {
                calories = (int)val;
            }
            else if (nut.ToLower().Contains("total lipid"))
            {
                totalFat = (int)val;
            }
            else if (nut.ToLower().Contains("total saturated"))
            {
                saturatedFat = (int)val;
            }
            else if (nut.ToLower().Contains("total trans"))
            {
                transFat = (int)val;
            }
            else if (nut.ToLower().Contains("cholesterol"))
            {
                cholestrol = (int)val;
            }
            else if (nut.ToLower().Contains("sodium"))
            {
                sodium = (int)val;
            }
            else if (nut.ToLower().Contains("carbohydrate"))
            {
                carbs = (int)val;
            }
            else if (nut.ToLower().Contains("fiber, total dietary"))
            {
                dietaryFiber = (int)val;
            }
            else if (nut.ToLower().Contains("sugars, total"))
            {
                totalSugars = (int)val;
            }
            else if (nut.ToLower().Contains("protein"))
            {
                protein = (int)val;
            }
            else
            {
                additionalNutrients.Add((new Nutrient(nut), val));
            }
        }
    }

    //********************************************************************************************//
    //Below are classes that are used to take the Json response from FDC API and turn them into objects to be parsed

    /// <summary>
    /// An class object that will be passed as a parameter for deserializing the returned Json from FDC API
    /// </summary>
    public class FoodInfoJson
    {
        public int totalHits { set; get; }  //amount of results from the Query
        public List<Food> foods { get; set; }//list of food objects from the Query

        override public string ToString()
        {
            string str = "";
            foreach (Food food in foods)
            {
                str += foods.ToString();
            }
            return str;
        }
    }

    /// <summary>
    /// A class object that holds the values for a food product returned from the FDC API
    /// </summary>
    public class Food
    {
        public string lowercaseDescription { get; set; }
        public string gtinUpc { get; set; }
        public string brandOwner { get; set; }
        public string brandName { get; set; }
        public string subbrandName { get; set; }
        public string ingredients { get; set; }
        public double servingSize { get; set; }

        public List<FoodNutrient> foodNutrients { get; set; }

        override public string ToString()
        {
            string str = lowercaseDescription + " " +
                    gtinUpc + " " +
                    brandOwner + " " +
                    brandName + " " +
                    subbrandName + " " +
                    servingSize + " " +
                    ingredients + " ";
            foreach (FoodNutrient foodNutrient in foodNutrients)
            {
                str = str + foodNutrient.ToString();
            }
            return str;
        }

    }

    /// <summary>
    /// A class object that holds the values for a nutrient 
    /// </summary>
    public class FoodNutrient
    {
        public string nutrientName { get; set; }
        public string unitName { get; set; }
        public double value { get; set; }

        override public string ToString()
        {
            return nutrientName + " " + unitName + " " + value + ",";
        }
    }
}
