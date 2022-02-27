using Food_Class_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{
    public class Ingredient
    {
        public string ingredientName { get; set; }
        
        public string ingredientID { get; set; }
        public string ingredientDescription { get; set; }
        public string ingredientShortName { get; set; }
        public Ingredient()
        {

        }

    }
}
