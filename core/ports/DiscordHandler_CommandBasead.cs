using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.ports
{
    internal class DiscordHandler_CommandBasead : IDiscordHandler
    {
        private ITextCommandHandler textCommandHandler;

        public DiscordHandler_CommandBasead(ITextCommandHandler textCommandHandler)
        {
            this.textCommandHandler = textCommandHandler;
        }

        public async Task OnGuildUserJoin(IDiscordService discord, UserModel user, ulong guildId)
        {

        }

        public async Task OnGuildUserLeft(IDiscordService discord, UserModel user, ulong guildId)
        {

        }

        public async Task OnMessageCreate(IDiscordService discord, MessageModel msg)
        {
            await textCommandHandler.ProcessCommand(discord, msg);
        }

        public async Task OnMessageEdit(IDiscordService discord, MessageModel msg)
        {
            await textCommandHandler.ProcessCommand(discord, msg);
        }
    }
}
