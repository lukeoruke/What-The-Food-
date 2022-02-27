using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Class_Library
{
    internal interface INutritionLabel
    {
        [Key]
        string barcode { get; }
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
        Double Biotin { get; set; }
        Double Choline { get; set; }
        Double Folate { get; set; }
        Double Niacin { get; set; }
        Double PantothenicAcid { get; set; }
        Double Riboflavin { get; set; }
        Double Thiamin { get; set; }
        Double A { get; set; }
        Double B6 { get; set; }
        Double B12 { get; set; }
        Double C { get; set; }
        Double D { get; set; }
        Double E { get; set; }
        Double K { get; set; }
        Double Calcium { get; set; }
        Double Chloride { get; set; }
        Double Chromium { get; set; }
        Double Copper { get; set; }
        Double Iodine { get; set; }
        Double Iron { get; set; }
        Double Magnesium { get; set; }
        Double Manganese { get; set; }
        Double Molybdenum { get; set; }
        Double Phosphorus { get; set; }
        Double Potassium { get; set; }
        Double Selenium { get; set; }
        Double Zinc { get; set; }
        ArrayList IngredientsList<String>();


    }
}
