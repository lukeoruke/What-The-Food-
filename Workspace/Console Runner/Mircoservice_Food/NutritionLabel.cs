

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Console_Runner.FoodService;

public class NutritionLabel
{
    [ForeignKey("Barcode")]
    public string Barcode { get; set; }

    private int _calories;
    private int _servings;
    private double _servingSize;
    private int _totalFat;
    private int _saturatedFat;
    private int _transFat;
    private int _cholesterol;
    private int _sodium;
    private int _totalCarbohydrate;
    private int _dietaryFiber;
    private int _totalSugars;
    private int _addedSugar;
    private int _protein;
    private List<(Nutrient, float)> _nutrients = new();

    public NutritionLabel()
    {

    }
    public NutritionLabel(int calories, int servings, double servingSize,
        int totalFat, int saturatedFat, int transFat, int cholestrol, int sodium,
        int totalCarbohydrate, int dietaryFiber, int totalSugars, int addedSugar,
        int protein, List<(Nutrient, float)> additionalNutrients, string barcode) {
        Calories = calories;
        Servings = servings;
        ServingSize = servingSize;
        TotalFat = totalFat;
        SaturatedFat = saturatedFat;
        TransFat = transFat;
        Cholesterol = cholestrol;
        Sodium = sodium;
        TotalCarbohydrate = totalCarbohydrate;
        DietaryFiber = dietaryFiber;
        TotalSugars = totalSugars;
        AddedSugar = addedSugar;
        Protein = protein;
        _nutrients = additionalNutrients;
        Barcode = barcode;
    }
    public List<(Nutrient, float)> GetNutrientList()
    {
        return _nutrients;
    }
    public int Calories           
    { 
        get
        {
            return _calories;
        }
        set 
        {
            if(value >= 0) _calories = value;
            else throw new ArgumentOutOfRangeException("Calories must be nonnegative!");
        }
    }
    public int Servings           
    {
        get
        {
            return _servings;
        }
        set
        {
            if(value > 0) _servings = value;
            else throw new ArgumentOutOfRangeException("Servings must be positive nonzero!");
        }
    }
    public double ServingSize     
    {
        get
        {
            return _servingSize;
        }
        set 
        {
            if(value > 0) _servingSize = value;
            else throw new ArgumentOutOfRangeException("ServingSize must be positive nonzero!");
        }
    }
    public int TotalFat           
    {
        get
        {
            return _totalFat;
        } 
        set 
        {
            if(value >= 0) _totalFat = value;
            else throw new ArgumentOutOfRangeException("TotalFat must be nonnegative!");
        }
    }
    public int SaturatedFat
    {
        get
        {
            return _saturatedFat;
        }
        set 
        {
            if(value >= 0) _saturatedFat = value;
            else throw new ArgumentOutOfRangeException("SaturatedFat must be nonnegative!");
        }
    }
    public int TransFat           
    {
        get
        {
            return _transFat;
        } 
        set
        {
            if(value >= 0) _transFat = value;
            else throw new ArgumentOutOfRangeException("TransFat must be nonnegative!");
        }
    }
    public int Cholesterol        
    {
        get
        {
            return _cholesterol;
        } 
        set
        {
            if(value >= 0) _cholesterol = value;
            else throw new ArgumentOutOfRangeException("Cholesterol must be nonnegative!");
        }
    }

    public int Sodium             
    {
        get
        {
            return _sodium;
        } 
        set
        {
            if(value >= 0) _sodium = value;
            else throw new ArgumentOutOfRangeException("Sodium must be nonnegative!");
        }
    }

    public int TotalCarbohydrate  
    {
        get
        {
            return _totalCarbohydrate;
        } 
        set
        {
            if(value >= 0) _totalCarbohydrate = value;
            else throw new ArgumentOutOfRangeException("TotalCarbohydrate must be nonnegative!");
        } 
    }

    public int DietaryFiber       
    {
        get
        {
            return _dietaryFiber;
        }
        set
        {
            if(value >= 0) _dietaryFiber = value;
            else throw new ArgumentOutOfRangeException("DietaryFiber must be nonnegative!");
        } 
    }

    public int TotalSugars        
    {
        get
        {
            return _totalSugars;
        }
        set
        {
            if(value >= 0) _totalSugars = value;
            else throw new ArgumentOutOfRangeException("TotalSugars must be nonnegative!");
        } 
    }

    public int AddedSugar         
    {
        get
        {
            return _addedSugar;
        }
        set 
        {
            if(value >= 0) _addedSugar = value;
            else throw new ArgumentOutOfRangeException("AddedSugar must be nonnegative!");
        } 
    }

    public int Protein            
    {
        get
        {
            return _protein;
        }
        set 
        {
            if(value >= 0) _protein = value;
            else throw new ArgumentOutOfRangeException("Protein must be nonnegative!");
        } 
    }

    public List<Ingredient> IngredientsList<String>()
    {
        throw new NotImplementedException();
    }

    public bool AddNutrient((Nutrient, float) nutrient)
    {
        _nutrients.Add(nutrient);
        return true;
    }

    public string FormatJsonString()
    {
        string str = $"\"Calories\":\"{Calories}\"," +
            $"\"Servings\":\"{Servings}\"," +
            $"\"ServingSize\":\"{ServingSize}\"," +
            $"\"TotalFat\":\"{TotalFat}\"," +
            $"\"SaturatedFat\":\"{SaturatedFat}\"," +
            $"\"TransFat\":\"{TransFat}\"," +
            $"\"Cholesterol\":\"{Cholesterol}\"," +
            $"\"Sodium\":\"{Sodium}\"," +
            $"\"TotalCarbohydrate\":\"{TotalCarbohydrate}\"," +
            $"\"DietaryFiber\":\"{DietaryFiber}\"," +
            $"\"TotalSugars\":\"{TotalSugars}\"," +
            $"\"AddedSugar\":\"{AddedSugar}\"," +
            $"\"Protein\":\"{Protein}\",";
        string listNut = "\"Nutrients\": [";
        string listNutAmt = "\"NutrientsAmount\": [";

        for (int i = 0; i < _nutrients.Count; i++)
        {
            listNut += $"\"{_nutrients[i].Item1.Name}\"";
            listNutAmt += $"\"{_nutrients[i].Item2}\"";

            if (i < _nutrients.Count - 1)
            {
                listNut += ",";
                listNutAmt += ",";
            }
            else if(i == _nutrients.Count - 1)
            {
                listNut += "]";
                listNutAmt += "]";
            }
        }

        str += listNut + ", " + listNutAmt;


        return str;
    }
}
