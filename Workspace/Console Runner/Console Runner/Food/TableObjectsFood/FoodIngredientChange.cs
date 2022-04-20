using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public class FoodIngredientChange : FoodUpdate
    {
        public IngredientUpdateList AddedIngredients { get; set; }
        public IngredientUpdateList RemovedIngredients { get; set; }

        public FoodIngredientChange() : base(){

        }

        public FoodIngredientChange(FoodItem foodItem, DateTime updateTime, string message, List<Ingredient> addedIngredients, List<Ingredient> removedIngredients)
            : base(foodItem, updateTime, message)
        {
            AddedIngredients = new IngredientUpdateList(addedIngredients);
            RemovedIngredients = new IngredientUpdateList(removedIngredients);
        }

        public FoodIngredientChange(FoodItem foodItem, DateTime updateTime, string message, IngredientUpdateList addedIngredients, IngredientUpdateList removedIngredients)
            : base(foodItem, updateTime, message)
        {
            AddedIngredients = addedIngredients;
            RemovedIngredients = removedIngredients;
        }
    }

    public class IngredientUpdateList
    {
        public List<Ingredient> Ingredients { get; set; }
        public int FoodIngredientChangeId { get; set; }

        public IngredientUpdateList()
        {
            Ingredients = new List<Ingredient>();
        }

        public IngredientUpdateList(List<Ingredient> ingredients)
        {
            Ingredients = ingredients;
        }
    }
}
