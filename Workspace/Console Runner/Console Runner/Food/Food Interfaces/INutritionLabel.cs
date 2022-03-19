using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService;

internal interface INutritionLabel
{
    [Key]
    string Barcode { get; }
    int Calories { get; set; }
    int Servings { get; set; }
    Double ServingSize { get; set; }
    int TotalFat { get; set; }
    int SaturatedFat { get; set; }
    int TransFat { get; set; }
    int Cholesterol { get; set; }
    int Sodium  { get; set; }
    int TotalCarbohydrate { get; set; }
    int DietaryFiber { get; set; }
    int TotalSugars { get; set; }
    int AddedSugar { get; set; }
    int Protein { get; set; }
    List<Nutrient> Nutrients { get; }
    Boolean AddNutrient (Nutrient nutrient);
    ArrayList IngredientsList<String>();


}
