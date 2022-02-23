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
            Name = name;
            Description = description;
            ShorterName = shorterName;
        }

        //Property Implementation
        [System.ComponentModel.DataAnnotations.Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShorterName { get; set; }
    }
}
