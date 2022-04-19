using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public class FoodIngredientChange : FoodUpdate
    {
        public List<Ingredient> AddedIngredients { get; set; }
        public List<Ingredient> RemovedIngredients { get; set; }

        public FoodIngredientChange() : base(){
            
        }

        public FoodIngredientChange(FoodItem foodItem, DateTime updateTime, string message, List<Ingredient> addedIngredients, List<Ingredient> removedIngredients) 
            : base (foodItem, updateTime, message)
        {
            AddedIngredients = addedIngredients;
            RemovedIngredients = removedIngredients;
        }
    }
}
