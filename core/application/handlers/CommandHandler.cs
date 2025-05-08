using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;

namespace O_Vigia_Docker.core.application.handlers
{
    internal class CommandHandler
    {
        protected ILogHandler _logHandler { get; private set; }
        protected IRepository _repository { get; private set; }
        protected IDiscordService _discord { get; private set; }
        protected MessageModel _msg { get; private set; }
        protected string[] _args { get; private set; }
        protected string _cleandContent { get; private set; }

        public void SetArgs(ILogHandler logHandler, IRepository repository, IDiscordService discord, MessageModel msg, string[] args, string cleandContent)
        {
            this._logHandler = logHandler;
            this._repository = repository;
            this._discord = discord;
            this._msg = msg;
            this._args = args;
            this._cleandContent = cleandContent;
        }
    }
}
