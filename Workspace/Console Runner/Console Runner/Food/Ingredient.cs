using Food_Class_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public class Ingredient
    {
        public string IngredientName { get; set; }
        [ForeignKey("IngredientID")]
        public LabelIngredient IngredientID { get; set; } = new LabelIngredient();
        //public string IngredientID { get; set; }
        public string IngredientDescription { get; set; }
        public string IngredientShortName { get; set; }
        public Ingredient()
        {

        }

        public Ingredient(string ingredientName, string ingredientShortName, string ingredientDescription)
        {
            IngredientName = ingredientName;
            IngredientDescription = ingredientDescription;
            IngredientShortName = ingredientShortName;

        }

    }
}
