

namespace Console_Runner.FoodService;

public class Ingredient
{
    public string IngredientName { get; set; }
    public int IngredientID { get; set; }

    public string AlternateName { get; set; }
    //public string IngredientID { get; set; }
    public string IngredientDescription { get; set; }
    public Ingredient()
    {

    }

    public Ingredient(string ingredientName, string alternateName,string ingredientDescription)
    {
        IngredientName = ingredientName;
        IngredientDescription = ingredientDescription;
        AlternateName = alternateName;

    }
}
