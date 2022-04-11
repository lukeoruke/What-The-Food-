using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Logging;

namespace Console_Runner.FoodService
{
    public static class FoodServiceLocator
    {
        public enum DataStoreType
        {
            InMemory,
            EntityFramework
        }
        public static FoodDBOperations GetFoodService(DataStoreType type)
        {
            LogService logger;
            switch (type)
            {
                case DataStoreType.InMemory:
                    logger = LogServiceLocator.GetLogService(LogServiceLocator.DataStoreType.InMemory);
                    break;
                case DataStoreType.EntityFramework:
                    logger = LogServiceLocator.GetLogService(LogServiceLocator.DataStoreType.EntityFramework);
                    break;
                default:
                    logger = LogServiceLocator.GetLogService(LogServiceLocator.DataStoreType.InMemory);
                    break;
            }
            IFoodGateway foodGateway;
            switch (type)
            {
                case DataStoreType.InMemory:
                    foodGateway = new MemFoodGateway();
                    break;
                case DataStoreType.EntityFramework:
                    foodGateway = new EFFoodGateway(LogServiceLocator.GetLogService(LogServiceLocator.DataStoreType.EntityFramework));
                    break;
                default:
                    foodGateway = new MemFoodGateway();
                    break;
            }
            return new FoodDBOperations(foodGateway, logger);
        }
    }
}
