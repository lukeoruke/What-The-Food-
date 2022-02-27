using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Class_Library
{
    public class NutritionLabel : INutritionLabel
    {
        [Key]
        public string barcode { get;}
        public NutritionLabel()
        {
        }
        public NutritionLabel(int calories, int servings, Double servingSize,
            int totalFat, int saturatedFat, int transFat, int cholestrol, int sodium,
            int totalCarbohydrate, int dietaryFiber, int totalSugars, int addedSugar,
            int protein, string labelID) {
            this.Calories = calories;
            this.Servings = servings;
            this.ServingSize = servingSize;
            this.TotalFat = totalFat;
            this.SaturatedFat = saturatedFat;
            this.SaturatedFat = transFat;
            this.Cholesterol = cholestrol;
            this.Sodium = sodium;
            this.TotalCarbohydrate = totalCarbohydrate;
            this.DietaryFiber = dietaryFiber;
            this.TotalSugars = totalSugars;
            this.AddedSugar = addedSugar;
            this.Protein = protein;
            this.barcode = labelID;
        }
 
        public int Calories { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Servings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double ServingSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TotalFat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SaturatedFat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TransFat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Cholesterol { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Sodium { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TotalCarbohydrate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int DietaryFiber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TotalSugars { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int AddedSugar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Protein { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Biotin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Choline { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Folate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Niacin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double PantothenicAcid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Riboflavin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Thiamin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double A { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double B6 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double B12 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double C { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double D { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double E { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double K { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Calcium { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Chloride { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Chromium { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Copper { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Iodine { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Iron { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Magnesium { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Manganese { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Molybdenum { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Phosphorus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Potassium { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Selenium { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Zinc { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ArrayList IngredientsList<String>()
        {
            throw new NotImplementedException();
        }
    }
}
