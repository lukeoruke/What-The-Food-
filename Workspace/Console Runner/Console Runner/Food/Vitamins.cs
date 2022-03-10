﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{
    public class Vitamins
    {
        public string Barcode { get; set; }
        public string VitaminName { get; set; }
        public int Percentage { get; set; }
        public Vitamins()
        {
        }
        public Vitamins(string barcode, string vitaminName)
        {
            Barcode = barcode;
            VitaminName = vitaminName;
        }
        public Vitamins(string barcode, string vitaminName, int percentage)
        {
            Barcode = barcode;
            VitaminName = vitaminName;
            Percentage = percentage;
        }
    }
}
