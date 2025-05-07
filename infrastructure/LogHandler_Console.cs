using O_Vigia.core.ports.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.infrastructure
{
    internal class LogHandler_Console : ILogHandler
    {
        public Task AddLog(string moduleName, string message, Exception? exception = null)
        {
            Console.WriteLine($"[{moduleName}] {message}.");

            if (exception == null)
                return Task.CompletedTask;

            Console.WriteLine($"{exception.Source}");
            Console.WriteLine($"{exception.InnerException}");
            return Task.CompletedTask;
        }
    }
}
