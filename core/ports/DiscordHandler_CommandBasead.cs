using O_Vigia.configs;
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
            await discord.SendMessage(AppSettings.chlId_Registro, new MessageModel() { content = $"Bem Vindo a o Servidor. <@{user.id}>" });
        }

        public async Task OnGuildUserLeft(IDiscordService discord, UserModel user, ulong guildId)
        {
            await discord.SendMessage(AppSettings.chlId_Registro, new MessageModel() { content = $"Deus, que pena tivemos que nós separar nossas jornadas. <@{user.id}>" });
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
