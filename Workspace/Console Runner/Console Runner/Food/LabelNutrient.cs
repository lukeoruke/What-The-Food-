using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{
    public class LabelNutrient
    {
        public string Barcode { get; set; }

        public string NutrientID { get; set; }

        public string NutrientPercentage { get; set; }

        //Constructor
        public LabelNutrient()
        {

        }

        public LabelNutrient(string barcode, string NID, string percent)
        {
            Barcode = barcode;
            NutrientID = NID;
            NutrientPercentage = percent;
        }
    }
}
