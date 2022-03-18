﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Class_Library
{
    public interface IIngredient
    {
        // Property Signatures:
        string ingredientName { get; set; }
        string description { get; set; }
        string shorterName { get; set; }

    }
}