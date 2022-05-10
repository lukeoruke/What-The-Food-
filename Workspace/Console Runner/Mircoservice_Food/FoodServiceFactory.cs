using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Logging;

namespace Console_Runner.FoodService
{
    public static class FoodServiceFactory
    {
        public enum DataStoreType
        {
            InMemory,
            EntityFramework
        }
        public static FoodDBOperations GetFoodService(DataStoreType type)
        {
            IFoodGateway foodGateway;
            IFoodUpdateGateway foodUpdateGateway;
            switch (type)
            {
                case DataStoreType.InMemory:
                    foodGateway = new MemFoodGateway();
                    foodUpdateGateway = new MemFoodUpdateGateway();
                    break;
                case DataStoreType.EntityFramework:
                    foodGateway = new EFFoodGateway();
                    foodUpdateGateway = new EFFoodUpdateGateway();
                    break;
                default:
                    foodGateway = new MemFoodGateway();
                    foodUpdateGateway = new MemFoodUpdateGateway();
                    break;
            }
            return new FoodDBOperations(foodGateway, foodUpdateGateway);
        }
    }
}
