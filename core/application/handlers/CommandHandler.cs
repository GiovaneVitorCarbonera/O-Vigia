using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.application.handlers
{
    internal class CommandHandler
    {
        protected ILogHandler logHandler { get; private set; }
        protected IRepository repository { get; private set; }
        protected IDiscordService discord { get; private set; }
        protected MessageModel msg { get; private set; }
        protected string[] args { get; private set; }

        public void SetArgs(ILogHandler logHandler, IRepository repository, IDiscordService discord, MessageModel msg, string[] args)
        {
            this.logHandler = logHandler;
            this.repository = repository;
            this.discord = discord;
            this.msg = msg;
            this.args = args;
        }
    }
}
