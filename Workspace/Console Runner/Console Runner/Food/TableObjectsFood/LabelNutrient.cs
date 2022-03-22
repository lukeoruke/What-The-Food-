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
    public int NutrientID { get; set; }

    public float NutrientPercentage { get; set; }

    //Constructor
    public LabelNutrient()
    {

    }

    public LabelNutrient(string barcode, int NID, float percent)
    {
        Barcode = barcode;
        NutrientID = NID;
        NutrientPercentage = percent;
    }
}
