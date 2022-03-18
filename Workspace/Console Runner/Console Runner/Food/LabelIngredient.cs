﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//TODO: this creates redundency. Why make the same food item over and over again(MQ)
namespace Console_Runner.Food
{
    //TODO: Perhaps add abstraction to this class. What should we add to increase security of an Ingredient object
    public class LabelIngredient
    {

        //Property Implementation
        public string Barcode { get; set; }
        public string IngredientID { get; set; }

        //Constructor
        public LabelIngredient()
        {

        }

        public LabelIngredient(string barcode, string ingredientID)
        {
            Barcode = barcode;
            IngredientID = ingredientID;
        }

    }
}
