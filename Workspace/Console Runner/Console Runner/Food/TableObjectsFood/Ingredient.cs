using Food_Class_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService;

public class Ingredient
{
    public string IngredientName { get; set; }
    public int IngredientID { get; set; }

    public string AlternateName { get; set; }
    //public string IngredientID { get; set; }
    public string IngredientDescription { get; set; }
    public Ingredient()
    {

    }

    public Ingredient(string ingredientName, string alternateName,string ingredientDescription)
    {
        IngredientName = ingredientName;
        IngredientDescription = ingredientDescription;
        AlternateName = alternateName;

    }

}
