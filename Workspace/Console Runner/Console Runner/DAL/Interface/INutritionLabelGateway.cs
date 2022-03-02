using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Food;

namespace Console_Runner.DAL
{
    public interface INutritionLabelGateway
    {
        public NutritionLabel RetrieveNutritionLabel(FoodItem food);
    }
}
