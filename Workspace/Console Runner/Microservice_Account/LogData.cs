using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.Logging
{
    public struct LogData
    {
        public LogLevel LogLevel;
        public Category Category;
        public DateTime Timestamp;
        public string Message;
    }
}
