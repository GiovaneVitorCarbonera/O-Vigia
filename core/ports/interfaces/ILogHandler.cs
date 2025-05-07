using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.ports.interfaces
{
    internal interface ILogHandler
    {
        Task AddLog(string moduleName, string message, Exception? exception = null);
    }
}
