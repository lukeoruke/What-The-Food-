

namespace Console_Runner.FoodService;

public class Nutrient
{
    public int NutrientID { get; set; }
    public string Name { get; set; }
    public Nutrient()
    {

    }
    public Nutrient(string vitaminName)
    {
        Name = vitaminName;
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