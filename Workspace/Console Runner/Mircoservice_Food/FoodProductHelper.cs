using Console_Runner.FoodService;

namespace Mircoservice_Food
{
    public class FoodProductHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ingredientList"></param>
        /// <returns></returns>
        public static string FormatIngredientsJsonString(List<Ingredient> ingredientList, List<Ingredient> flaggedList)
        {
            string strNameList = "\"IngredientName\": [";
            string strAltList = "\"IngredientAlternateName\": [";
            string strDescList = "\"IngredientDescription\": [";
            string flaggedItemList = "\"FlaggedItems\": [";
            for (int i = 0; i < flaggedList.Count; i++)
            {
                flaggedItemList += $"\"{flaggedList[i].IngredientName}\"";
                if (i < flaggedList.Count - 1)
                {
                    flaggedItemList += ",";
                }
                else if (i == flaggedList.Count - 1)
                {
                    flaggedItemList += "]";
                }

            }

            for (int i = 0; i < ingredientList.Count; i++)
            {
                strNameList += $"\"{ingredientList[i].IngredientName}\"";
                strAltList += $"\"{ingredientList[i].AlternateName}\"";
                strDescList += $"\"{ingredientList[i].IngredientDescription}\"";
                if (i < ingredientList.Count - 1)
                {
                    strNameList += ",";
                    strAltList += ",";
                    strDescList += ",";
                }
                else if (i == ingredientList.Count - 1)
                {
                    strNameList += "]";
                    strAltList += "]";
                    strDescList += "]";
                }
            }
            if (flaggedList.Count == 0)
            {
                return strNameList + ", " + strAltList + ", " + strDescList;
            }
            else
            {
                return strNameList + ", " + strAltList + ", " + strDescList + ", " + flaggedItemList;
            }

        }
    }
}
