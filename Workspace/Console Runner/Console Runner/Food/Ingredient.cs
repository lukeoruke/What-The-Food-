using Food_Class_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{
    public class Ingredient
    {
        public string ingredientName { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string ingredientID { get; set; }
        public string ingredientDescription { get; set; }
        public string ingredientShortName { get; set; }
        public Ingredient()
        {

        }

    }
}
