using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Logging.Testing;

namespace Console_Runner.Logging
{
    public static class LogServiceFactory
    {
        public enum DataStoreType
        {
            InMemory,
            EntityFramework
        }
        /// <summary>
        /// Returns an instance of LogService with data store implementation depending on input.
        /// </summary>
        /// <param name="type">The data store implementation desired - 0 for in-memory, 1 for Entity Framework. Defaults to in-memory if invalid input.</param>
        /// <returns>A LogService instance with the desired data store implementation.</returns>
        public static LogService GetLogService(DataStoreType type)
        {
            ILogGateway logGateway;
            IUserIDGateway uidGateway;
            switch (type)
            {
                case DataStoreType.InMemory:
                    logGateway = new MemLogGateway();
                    uidGateway = new MemUserIdentifierGateway();
                    break;
                case DataStoreType.EntityFramework:
                    logGateway = new EFLogGateway();
                    uidGateway = new EFUserIdentifierGateway();
                    break;
                default:
                    logGateway = new MemLogGateway();
                    uidGateway = new MemUserIdentifierGateway();
                    break;
            }
            return new LogService(logGateway, uidGateway);
        }
    }
}
