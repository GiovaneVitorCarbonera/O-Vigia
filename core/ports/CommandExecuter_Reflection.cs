using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.handlers;
using O_Vigia_Docker.core.ports.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.ports
{
    internal class CommandExecuter_Reflection : ICommandExecutor
    {
        ILogHandler _logHandler;
        IRepository _repository;

        public CommandExecuter_Reflection(ILogHandler logHandler, IRepository repository)
        {
            this._logHandler = logHandler;
            this._repository = repository;
        }

        public async Task RunCommand(IDiscordService discord, MethodInfo mth, Type ty, MessageModel msg, string[] args, string cleanContentForCommands)
        {
            try
            {
                object instance = Activator.CreateInstance(ty);
                if (instance == null || !(instance is CommandHandler)) return;

                CommandHandler CB = (CommandHandler)instance;
                CB.SetArgs(_logHandler, _repository, discord, msg, args, cleanContentForCommands);

                string rsmsg = await (Task<string>)mth.Invoke(CB, null);

                if (!string.IsNullOrWhiteSpace(rsmsg))
                {
                    await discord.SendMessage(msg.loc.channelId, new O_Vigia.core.application.models.MessageModel()
                    {
                        content = rsmsg,
                        msgReplyId = msg.loc.messageId,
                    });
                }
            }
            catch (Exception ex)
            {
                await _logHandler.AddLog(this.GetType().Name, ex.Message, ex);
            }
        }
    }
}
