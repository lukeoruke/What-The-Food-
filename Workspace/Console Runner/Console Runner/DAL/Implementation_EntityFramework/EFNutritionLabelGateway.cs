using Console_Runner.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.DAL
{
    public class EFNutritionLabelGateway : INutritionLabelGateway
    {
        private Context _efContext;

        public EFNutritionLabelGateway(Context dbContext)
        {
            _efContext = dbContext;
        }
        public NutritionLabel? RetrieveNutritionLabel(FoodItem food)
        {
            return _efContext.NutritionLabels.Find(food.barcode);
        }
    }
}
