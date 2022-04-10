
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Console_Runner.FoodService
{
    public class EFFoodGateway : IFoodGateway
    {
        private readonly ContextFoodDB _efContext;



        public EFFoodGateway()
        {
            _efContext = new ContextFoodDB();
        }

        public async Task<List<Ingredient>> GetIngredientBySearchAsync(string search, int skip, int take)
        {
            List<Ingredient> results = await _efContext.Ingredients.Where(x => x.IngredientName.Contains(search)).OrderBy(x => x.IngredientName).ToListAsync();
            return results;
        }

        public async Task<List<Ingredient>>RetrieveNIngredientsAsync(int skip, int take)
        {
            List<Ingredient> results = await _efContext.Ingredients.OrderBy(x => x.IngredientName).Skip(skip).Take(take).ToListAsync();
            return results;
        }

        public async Task<bool> AddLabelIngredientAsync(LabelIngredient labelIngredient)
        {
            try
            {
                
                if (!_efContext.LabelIngredients.Contains(labelIngredient))
                    {
                        await _efContext.LabelIngredients.AddAsync(labelIngredient);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<bool> AddLabelNutrientAsync(LabelNutrient labelNutrient)
        {
            try
            {

                if (!_efContext.LabelNutrients.Contains(labelNutrient))
                {
                    await _efContext.LabelNutrients.AddAsync(labelNutrient);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


       

        public async Task<bool> AddFoodItemAsync(FoodItem foodItem)
        {
            try
            {
                await _efContext.FoodItem.AddAsync(foodItem);
                await _efContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddNutritionLabelAsync(NutritionLabel nutritionLabel)
        {
            try
            {
                await _efContext.NutritionLabel.AddAsync(nutritionLabel);
                await _efContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddNutrientAsync(Nutrient nutrient)
        {
            try
            {
                await _efContext.Nutrient.AddAsync(nutrient);
                await _efContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AddIngredientAsync(Ingredient ingredient)
        {
            try
            {
                await _efContext.Ingredients.AddAsync(ingredient);
                await _efContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveIngredient(Ingredient ingredient)
        {
            try
            {
                _efContext.Ingredients.Remove(ingredient);
                _efContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Ingredient>> RetrieveIngredientListAsync(string barcode)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            var ListOfIngredients = _efContext.LabelIngredients.Where(r => r.Barcode == barcode).ToList();
            for(int i = 0; i < ListOfIngredients.Count(); i++)
            {
                ingredients.Add(await _efContext.Ingredients.FindAsync(ListOfIngredients[0].IngredientID));
            }
/*            foreach(LabelIngredient ings in ListOfIngredients)
            {
                ingredients.Add(await _efContext.Ingredients.FindAsync(ings.IngredientID));
            }*/
            return ingredients;
        }

        public async Task<NutritionLabel?> RetrieveNutritionLabelAsync(string barcode)
        {
            return await _efContext.NutritionLabel.FindAsync(barcode);
        }

        public async Task<FoodItem?> RetrieveScannedFoodItemAsync(string barcode)
        {
            FoodItem? food = await _efContext.FoodItem.FindAsync(barcode);
            return food;
        }


        public async Task<List<LabelNutrient>> RetrieveLabelNutrientByBarcodeAsync(string barcode)
        {
            return _efContext.LabelNutrients.Where(r => r.Barcode == barcode).ToList();
        }

        public async Task<List<(Nutrient, float)>> RetrieveNutrientListByIDAsync(List<LabelNutrient> list)
        {
            List<(Nutrient?, float)> nutrientList = new();
            for (int i = 0; i < list.Count; i++)
            {
                 nutrientList.Add((await _efContext.Nutrient.FindAsync(list[i].NutrientID), list[i].NutrientPercentage));
            }
            return nutrientList;
        }
    }
}
