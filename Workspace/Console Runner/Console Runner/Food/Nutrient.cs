using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService
{
    public class Nutrient
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        private int _amount;
        public int Amount 
        {
            get
            {
                return _amount;
            }
            set
            {
                if(value > 0) _amount = value;
                else throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
        public Nutrient()
        {

        }
        public Nutrient(string barcode, string name)
        {
            Barcode = barcode;
            Name = name;
        }
        public Nutrient(string barcode, string vitaminName, int amount)
        {
            Barcode = barcode;
            Name = vitaminName;
            Amount = amount;
        }
    }
}

/* possible nutrients so far: 
 * Biotin
 * Choline
 * Folate
 * Niacin
 * PantothenicAcid
 * Riboflavin
 * Thiamin
 * VitaminA
 * VitaminB6
 * VitaminB12
 * VitaminC
 * VitaminD
 * VitaminE
 * VitaminK
 * Calcium
 * Chloride
 * Chromium
 * Copper
 * Iodine
 * Iron
 * Magnesium
 * Manganese
 * Molybdenum
 * Phosphorus
 * Potassium
 * Selenium
 * Zinc
 */