using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public class FoodIngredientChange : FoodUpdate
    {
        public List<IngredientUpdate> IngredientUpdates { get; set; }

        public FoodIngredientChange() : base(){

        }

        public FoodIngredientChange(FoodItem foodItem, DateOnly updateTime, string message, IEnumerable<Ingredient> addedIngredients, IEnumerable<Ingredient> removedIngredients)
            : base(foodItem, updateTime, message)
        {
            IngredientUpdates = new();
            IngredientUpdates.AddRange(addedIngredients.ToList().ConvertAll(ing => new IngredientUpdate(this, ing, true)));
            IngredientUpdates.AddRange(removedIngredients.ToList().ConvertAll(ing => new IngredientUpdate(this, ing, false)));
        }

        public FoodIngredientChange(FoodItem foodItem, DateOnly updateTime, string message, IEnumerable<IngredientUpdate> ingredientUpdates)
            : base(foodItem, updateTime, message)
        {
            IngredientUpdates = ingredientUpdates.ToList();
        }
    }

    public class IngredientUpdate
    {
        [JsonIgnore]
        public FoodIngredientChange FoodIngredientChange { get; set; }
        [JsonIgnore]
        public int FoodIngredientChangeId { get; set; }
        [JsonIgnore]
        public Ingredient Ingredient { get; set; }
        [JsonIgnore]
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public bool IsAdded { get; set; }

        public IngredientUpdate()
        {
        }

        public IngredientUpdate(FoodIngredientChange foodIngredientChange, Ingredient ingredient, bool isAdded)
        {
            FoodIngredientChange = foodIngredientChange;
            Ingredient = ingredient;
            IngredientName = ingredient.IngredientName;
            IsAdded = isAdded;
        }
    }
}
