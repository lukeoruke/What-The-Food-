using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food;

public class NutritionLabel : INutritionLabel
{
    [Key]
    public string barcode { get; set; }
    public NutritionLabel()
    {
    }
    public NutritionLabel(int calories, int servings, Double servingSize,
        int totalFat, int saturatedFat, int transFat, int cholestrol, int sodium,
        int totalCarbohydrate, int dietaryFiber, int totalSugars, int addedSugar,
        int protein, string barcode) {
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
        this.barcode = barcode;
    }

    public int Calories           
    { 
        get; 
        set {
            if(value >= 0) Calories = value;
            else throw new ArgumentOutOfRangeException("Calories must be nonnegative!");
        }
    }
    public int Servings           
    { 
        get; 
        set {
            if(value > 0) Servings = value;
            else throw new ArgumentOutOfRangeException("Servings must be positive nonzero!");
        }
    }
    public double ServingSize     
    {
        get; 
        set {
            if(value > 0) ServingSize = value;
            else throw new ArgumentOutOfRangeException("ServingSize must be positive nonzero!");
        }
    }
    public int TotalFat           
    { 
        get; 
        set {
            if(value >= 0) TotalFat = value;
            else throw new ArgumentOutOfRangeException("TotalFat must be nonnegative!");
        }
    }
    public int SaturatedFat
    { 
        get;
        set {
            if(value >= 0) SaturatedFat = value;
            else throw new ArgumentOutOfRangeException("SaturatedFat must be nonnegative!");
        }
    }
    public int TransFat           
    { 
        get; 
        set {
            if(value >= 0) TransFat = value;
            else throw new ArgumentOutOfRangeException("TransFat must be nonnegative!");
        }
    }
    public int Cholesterol        
    { 
        get; 
        set {
            if(value >= 0) Cholesterol = value;
            else throw new ArgumentOutOfRangeException("Cholesterol must be nonnegative!");
        }
    }

    public int Sodium             
    { 
        get; 
        set {
            if(value >= 0) Sodium = value;
            else throw new ArgumentOutOfRangeException("Sodium must be nonnegative!");
        }
    }

    public int TotalCarbohydrate  
    { 
        get; 
        set {
            if(value >= 0) TotalCarbohydrate = value;
            else throw new ArgumentOutOfRangeException("TotalCarbohydrate must be nonnegative!");
        } 
    }

    public int DietaryFiber       
    { 
        get; 
        set {
            if(value >= 0) DietaryFiber = value;
            else throw new ArgumentOutOfRangeException("DietaryFiber must be nonnegative!");
        } 
    }

    public int TotalSugars        
    { 
        get; 
        set {
            if(value >= 0) TotalSugars = value;
            else throw new ArgumentOutOfRangeException("TotalSugars must be nonnegative!");
        } 
    }

    public int AddedSugar         
    { 
        get; 
        set {
            if(value >= 0) AddedSugar = value;
            else throw new ArgumentOutOfRangeException("AddedSugar must be nonnegative!");
        } 
    }

    public int Protein            
    { 
        get; 
        set {
            if(value >= 0) Protein = value;
            else throw new ArgumentOutOfRangeException("Protein must be nonnegative!");
        } 
    }

    public double Biotin          
    { 
        get; 
        set {
            if(value >= 0) Biotin = value;
            else throw new ArgumentOutOfRangeException("Biotin must be nonnegative!");
        }  
    }

    public double Choline         
    { 
        get; 
        set {
            if(value >= 0) Choline = value;
            else throw new ArgumentOutOfRangeException("Choline must be nonnegative!");
        } 
    }

    public double Folate          
    { 
        get; 
        set {
            if(value >= 0) Folate = value;
            else throw new ArgumentOutOfRangeException("Folate must be nonnegative!");
        } 
    }

    public double Niacin          
    { 
        get; 
        set {
            if(value >= 0) Niacin = value;
            else throw new ArgumentOutOfRangeException("Niacin must be nonnegative!");
        } 
    }

    public double PantothenicAcid 
    { 
        get; 
        set {
            if(value >= 0) PantothenicAcid = value;
            else throw new ArgumentOutOfRangeException("PantothenicAcid must be nonnegative!");
        } 
    }

    public double Riboflavin      
    { 
        get; 
        set {
            if(value >= 0) Riboflavin = value;
            else throw new ArgumentOutOfRangeException("Riboflavin must be nonnegative!");
        } 
    }

    public double Thiamin         
    { 
        get; 
        set {
            if(value >= 0) Thiamin = value;
            else throw new ArgumentOutOfRangeException("Thiamin must be nonnegative!");
        } 
    }

    public double VitaminA               
    { 
        get; 
        set {
            if(value >= 0) VitaminA = value;
            else throw new ArgumentOutOfRangeException("VitaminA must be nonnegative!");
        } 
    }

    public double VitaminB6              
    { 
        get; 
        set {
            if(value >= 0) VitaminB6 = value;
            else throw new ArgumentOutOfRangeException("VitaminB6 must be nonnegative!");
        } 
    }

    public double VitaminB12             
    { 
        get; 
        set {
            if(value >= 0) VitaminB12 = value;
            else throw new ArgumentOutOfRangeException("VitaminB12 must be nonnegative!");
        } 
    }

    public double VitaminC               
    { 
        get; 
        set {
            if(value >= 0) VitaminC = value;
            else throw new ArgumentOutOfRangeException("VitaminC must be nonnegative!");
        } 
    }

    public double VitaminD               
    { 
        get; 
        set {
            if(value >= 0) VitaminD = value;
            else throw new ArgumentOutOfRangeException("VitaminD must be nonnegative!");
        } 
    }

    public double VitaminE               
    { 
        get; 
        set {
            if(value >= 0) VitaminE = value;
            else throw new ArgumentOutOfRangeException("VitaminE must be nonnegative!");
        } 
    }

    public double VitaminK               
    { 
        get; 
        set {
            if(value >= 0) VitaminK = value;
            else throw new ArgumentOutOfRangeException("VitaminK must be nonnegative!");
        } 
    }

    public double Calcium         
    { 
        get; 
        set {
            if(value >= 0) Calcium = value;
            else throw new ArgumentOutOfRangeException("Calcium must be nonnegative!");
        } 
    }

    public double Chloride        
    { 
        get; 
        set {
            if(value >= 0) Chloride = value;
            else throw new ArgumentOutOfRangeException("Chloride must be nonnegative!");
        } 
    }

    public double Chromium        
    { 
        get; 
        set {
            if(value >= 0) Chromium = value;
            else throw new ArgumentOutOfRangeException("Chromium must be nonnegative!");
        } 
    }

    public double Copper          
    { 
        get; 
        set {
            if(value >= 0) Copper = value;
            else throw new ArgumentOutOfRangeException("Copper must be nonnegative!");
        } 
    }

    public double Iodine          
    { 
        get; 
        set {
            if(value >= 0) Iodine = value;
            else throw new ArgumentOutOfRangeException("Iodine must be nonnegative!");
        } 
    }

    public double Iron            
    { 
        get; 
        set {
            if(value >= 0) Iron = value;
            else throw new ArgumentOutOfRangeException("Iron must be nonnegative!");
        } 
    }

    public double Magnesium       
    { 
        get; 
        set {
            if(value >= 0) Magnesium = value;
            else throw new ArgumentOutOfRangeException("Magnesium must be nonnegative!");
        } 
    }

    public double Manganese       
    { 
        get; 
        set {
            if(value >= 0) Manganese = value;
            else throw new ArgumentOutOfRangeException("Manganese must be nonnegative!");
        } 
    }

    public double Molybdenum      
    { 
        get; 
        set {
            if(value >= 0) Molybdenum = value;
            else throw new ArgumentOutOfRangeException("Molybdenum must be nonnegative!");
        } 
    }

    public double Phosphorus      
    { 
        get; 
        set {
            if(value >= 0) Phosphorus = value;
            else throw new ArgumentOutOfRangeException("Phosphorus must be nonnegative!");
        } 
    }

    public double Potassium       
    { 
        get; 
        set {
            if(value >= 0) Potassium = value;
            else throw new ArgumentOutOfRangeException("Potassium must be nonnegative!");
        } 
    }

    public double Selenium        
    { 
        get; 
        set {
            if(value >= 0) Selenium = value;
            else throw new ArgumentOutOfRangeException("Selenium must be nonnegative!");
        } 
    }

    public double Zinc            
    { 
        get; 
        set {
            if(value >= 0) Zinc = value;
            else throw new ArgumentOutOfRangeException("Zinc must be nonnegative!");
        } 
    }

    public ArrayList IngredientsList<String>()
    {
        throw new NotImplementedException();
    }
}
