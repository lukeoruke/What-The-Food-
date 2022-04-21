using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public class FoodIngredientChange : FoodUpdate
    {
        public List<IngredientUpdate> IngredientUpdates { get; set; }

        public FoodIngredientChange() : base(){

        }

        public FoodIngredientChange(FoodItem foodItem, DateTime updateTime, string message, List<Ingredient> addedIngredients, List<Ingredient> removedIngredients)
            : base(foodItem, updateTime, message)
        {
            IngredientUpdates = new();
            IngredientUpdates.AddRange(addedIngredients.ConvertAll(ing => new IngredientUpdate(this, ing, true)));
            IngredientUpdates.AddRange(removedIngredients.ConvertAll(ing => new IngredientUpdate(this, ing, false)));
        }

        public FoodIngredientChange(FoodItem foodItem, DateTime updateTime, string message, List<IngredientUpdate> ingredientUpdates)
            : base(foodItem, updateTime, message)
        {
            IngredientUpdates = ingredientUpdates;
        }
    }

    public class IngredientUpdate
    {
        public FoodIngredientChange FoodIngredientChange { get; set; }
        public int FoodIngredientChangeId { get; set; }
        public Ingredient Ingredient { get; set; }
        public int IngredientId { get; set; }
        public bool IsAdded { get; set; }

        public IngredientUpdate()
        {
        }

        public IngredientUpdate(FoodIngredientChange foodIngredientChange, Ingredient ingredient, bool isAdded)
        {
            FoodIngredientChange = foodIngredientChange;
            Ingredient = ingredient;
            IsAdded = isAdded;
        }
    }
}
