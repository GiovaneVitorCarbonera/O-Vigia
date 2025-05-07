using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.ports
{
    internal class TextCommandHandler_Reflection : ITextCommandHandler
    {
        private ILogHandler logHandler;
        private IRepository repository;

        public TextCommandHandler_Reflection(ILogHandler logHandler, IRepository repository)
        {
            this.logHandler = logHandler;
            this.repository = repository;
        }

        public async Task ProcessCommand(IDiscordService discord, MessageModel msg)
        {

        }
    }
}
