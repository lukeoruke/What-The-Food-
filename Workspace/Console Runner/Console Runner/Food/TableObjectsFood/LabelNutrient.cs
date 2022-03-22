using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.FoodService;

public class LabelNutrient
{
    [ForeignKey("Barcode")]
    public string Barcode { get; set; }

    [ForeignKey("Barcode")]
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
