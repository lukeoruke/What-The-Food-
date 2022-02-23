using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Class_Library
{
    internal interface IIngredient
    {
        // Property Signatures:
        string Name { get; set; }
        string Description { get; set; }
        string ShorterName { get; set; }

    }
}
