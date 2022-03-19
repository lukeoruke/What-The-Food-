﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//TODO: this creates redundency. Why make the same food item over and over again(MQ)
namespace Console_Runner.FoodService;

//TODO: Perhaps add abstraction to this class. What should we add to increase security of an Ingredient object
public class LabelIngredient
{

    //Property Implementation
    [ForeignKey("Barcode")]
    public FoodItem Barcode { get; set; } = new FoodItem();
    //public string Barcode { get; set; }

    public string IngredientID { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public string IngredientsPercentage { get; set; }

    //Constructor
    public LabelIngredient()
    {

    }

    public LabelIngredient(string barcode, string ingredientID, string percent)
    {
        Barcode = barcode;
        IngredientID = ingredientID;
        IngredientsPercentage = percent;
    }

}
