using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Class_Library
{
    //TODO: Perhaps add abstraction to this class. What should we add to increase security of an Ingredient object
        public class Ingredient : IIngredient
    {
        //Constructor
        public Ingredient()
        {
        }
        public Ingredient(string name, string description, string shorterName) {
            ingredientName = name;
            this.description = description;
            this.shorterName = shorterName;
        }

        //Property Implementation
        public string labelID;
        public string ingredientName { get; set; }
        public string description { get; set; }
        public string shorterName { get; set; }
    }
}
