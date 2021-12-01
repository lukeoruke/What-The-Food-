using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNoStress.WhatTheFood.Logging
{
    public interface ILogService
    {
        bool Log(string description);
        IList<string> GetAllLogs();
    }
}
