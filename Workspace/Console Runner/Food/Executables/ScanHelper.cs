using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;
using Console_Runner.FoodService;

namespace Food.Executables
{
    public class ScanHelper
    {
        //dependency injection
        private const string UM_CATEGORY = "Data Store";
        private readonly IFoodGateway _foodServiceGateway = new EFFoodGateway();
        private FoodDBOperations _foodDB;
        private IFormCollection formData;

        private readonly HttpClient client;
        private readonly string url = "https://api.nal.usda.gov/fdc/";
        private readonly string apiKey = "lpdKjKHTtT3zCxQebMkJgGkvxgWqmuD9OL2NqviF";
        private NutrientValues nutValues = new NutrientValues();

        public ScanHelper()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add("apiKey", apiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> SearchAndAdd(string barcode)
        {
            FoodInfoJson queryResponse = await GET(barcode); //search barcode from FDC API

            //new variable creation
            FoodItem newFood;
            NutritionLabel newLabel;
            foreach(Food food in queryResponse.foods)   //for every response we get we parse and add to our DB
            {
                //Reset reused varaibles
                List<Nutrient> newNutrients = new List<Nutrient>();
                nutValues.ClearValues();

                //make a new food object to be added to DB
                newFood = new FoodItem(food.gtinUpc, food.subbrandName, food.brandName, "");

                //for this food item go through all nutrients and note what they are and what their values are
                foreach (FoodNutrient nutrient in food.foodNutrients) {
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
                //await _foodDB.AddNewProductAsync(newFood, newLabel, newIngredients);
                Console.WriteLine(newFood.FormatJsonString());
                Console.WriteLine(newLabel.FormatJsonString());
                foreach (Ingredient i in newIngredients) {
                    Console.WriteLine(i.FormatIngredientsJsonString());
                }
            }

            return "good";
        }

        private async Task<FoodInfoJson> GET(string barcode)
        {
            try
            {
                //Console.WriteLine("Requesting!: " + client.BaseAddress + "v1/foods/search?query=" + barcode + "&api_key=" + apiKey);
                var streamTask = client.GetStreamAsync(client.BaseAddress + "v1/foods/search?query=" + barcode + "&api_key=" + apiKey);

                var values = await JsonSerializer.DeserializeAsync<FoodInfoJson>(await streamTask);
                //Console.WriteLine("printing values: " + values.foods[0].ToString());

                return values;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }
        
        public List<Ingredient> StringToIngredients(string str)
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            string temp = "";
            Boolean inParentheses = false;

            //get all ingredients from the string value
            while(str.Length > 0)   //this loop ensures that we properly get all food ingredients without cutting off too early
            {
                Console.WriteLine("str Length: " + str.Length);
                if (str[0] == '(')
                {
                    inParentheses = true;
                    temp += str[0];
                    str = str.Substring(1);
                }
                else if(str[0] == ')')
                {
                    inParentheses= false;
                    temp += str[0];
                    str = str.Substring(1);
                }
                else if (str[0] == ',' && inParentheses == false)
                {
                    ingredients.Add(new Ingredient(temp, "", ""));
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
        /// Formats a json file to a string
        /// </summary>
        /// <param name="ingredientList"></param>
        /// <returns></returns>
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
        public List<(Nutrient, float)> additionalNutrients { get; set; }

        public NutrientValues()
        {
            calories = totalFat = saturatedFat = transFat = cholestrol = sodium = 
                carbs = dietaryFiber = totalSugars = addedSugar = protein = 0;
            additionalNutrients = new List<(Nutrient, float)>();
        }

        public void ClearValues()
        {
            calories = totalFat = saturatedFat = transFat = cholestrol = sodium =
                carbs = dietaryFiber = totalSugars = addedSugar = protein = 0;
            additionalNutrients = new List<(Nutrient, float)>();
        }

        public void AddNutrientValue(string nut, float val)
        {
            if(nut.ToLower().Contains("energy"))
            {
                calories = (int)val;
            }
            else if(nut.ToLower().Contains("total lipid"))
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
    public class FoodInfoJson
    {
        public int totalHits { set; get; }
        public List<Food> foods { get; set; }

        override public string ToString()
        {
            string str = "";
            foreach(Food food in foods)
            {
                str += foods.ToString();
            }
            return str;
        }
    }

    public class Food
    {
        public string lowercasedescription {get; set;}
        public string gtinUpc { get; set; }
        public string brandOwner { get; set; }
        public string brandName { get; set; }
        public string subbrandName { get; set; }
        public string ingredients { get; set; }
        public double servingSize { get; set; }

        public List<FoodNutrient> foodNutrients { get; set; }

        override public string ToString()
        {
            string str = lowercasedescription + " " +
                    gtinUpc + " " +
                    brandOwner + " " +
                    brandName + " " +
                    subbrandName + " " +
                    servingSize + " " +
                    ingredients + " ";
            foreach(FoodNutrient foodNutrient in foodNutrients)
            {
                str = str + foodNutrient.ToString();
            }
            return str;
        }

    }

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
