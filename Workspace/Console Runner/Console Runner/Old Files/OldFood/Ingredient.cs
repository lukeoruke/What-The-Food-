using Food_Class_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Class_Library
{
    public class Ingredient
    {
        public string IngredientName { get; set; }
        
        public string IngredientID { get; set; }
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
