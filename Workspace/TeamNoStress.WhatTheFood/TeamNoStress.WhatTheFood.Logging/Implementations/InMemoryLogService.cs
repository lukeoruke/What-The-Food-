using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNoStress.WhatTheFood.Logging.Implementations
{
    public class InMemoryLogService : ILogService
    {
        private readonly IList<string> _logStore;

        public InMemoryLogService()
        {
            _logStore = new List<string>();
        }
        public bool Log(string description)
        {
            try
            {
                _logStore.Add($"{DateTime.UtcNow}-> {description}");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IList<string> GetAllLogs()
        {
            return _logStore;
        }
    }
}
