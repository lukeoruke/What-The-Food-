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
    public class LoggableResponse<T>
    {
        public T? Data { get; }
        public bool IsSuccessful { get; }
        public DateTime Timestamp { get; }
        public LogLevel LogLevel { get; }
        public Category Category { get; }
        public string Message { get; }
        public string? ExceptionType { get; }
        public List<LoggableResponse<object>> InnerResponses { get; }

        public LoggableResponse (T data, bool isSuccessful, string message, string exceptionType = null){
            Data = data;
            IsSuccessful = IsSuccessful;
            Message = message;
            ExceptionType = exceptionType;
        }

        public List<LogData> ToLogData()
        {
            List<LogData> toReturn = new();
            toReturn.Add(new LogData { LogLevel = this.LogLevel, Category = this.Category, Timestamp = this.Timestamp, Message = this.Message });
            foreach (var response in InnerResponses)
            {
                toReturn.AddRange(response.ToLogData());
            }
            return toReturn;
        }

    }
}
