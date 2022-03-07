using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{
    public class Vitamins
    {
        public string barcode;
        public string vitaminName;
        int percentage;
        public Vitamins()
        {
        }
        public Vitamins(string barcode, string vitaminName)
        {
            this.barcode = barcode;
            this.vitaminName = vitaminName;
        }
        public Vitamins(string barcode, string vitaminName, int percentage)
        {
            this.barcode = barcode;
            this.vitaminName = vitaminName;
            this.percentage = percentage;
        }
    }
}
