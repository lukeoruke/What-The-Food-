﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.DAL
{
    public interface IHistoryGateway
    {
        public bool AddHistoryItem(History historyItem);
    }
}