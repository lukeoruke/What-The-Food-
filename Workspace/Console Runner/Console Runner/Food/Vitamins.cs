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
        public string labelID;
        public string vitaminName;
        int percentage;
        public Vitamins()
        {
        }
        public Vitamins(string labelID, string vitaminName)
        {
            this.labelID = labelID;
            this.vitaminName = vitaminName;
        }
        public Vitamins(string labelID, string vitaminName, int percentage)
        {
            this.labelID = labelID;
            this.vitaminName = vitaminName;
            this.percentage = percentage;
        }
    }
}
