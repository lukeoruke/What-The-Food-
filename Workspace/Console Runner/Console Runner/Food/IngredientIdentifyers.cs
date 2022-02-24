using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//TODO: this creates redundency. Why make the same food item over and over again(MQ)
namespace Food_Class_Library
{
    //TODO: Perhaps add abstraction to this class. What should we add to increase security of an Ingredient object
        public class IngredientIdentifyers
    {
        //Constructor
        public IngredientIdentifyers()
        {
        }

        //Property Implementation
        public string labelID;
        public string ingredientID{ get; set; }

    }
}
