﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Food
{
   public class FoodItem
    {
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        //public string labelID { get; set; }
        public FoodItem()
        {

        }
        public FoodItem(string barcode, string productName, string companyName)
        {
            Barcode = barcode;
            ProductName = productName;
            CompanyName = companyName;
        }
       

    }
}
