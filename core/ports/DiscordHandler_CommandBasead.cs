using NetCord.Gateway;
using NetCord;
using O_Vigia.configs;
using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.ports.interfaces;
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
        private ITultorialController tultorialController;
        private IRepository repository;

        public DiscordHandler_CommandBasead(ITextCommandHandler textCommandHandler, IRepository repository, ITultorialController tultorialController)
        {
            this.textCommandHandler = textCommandHandler;
            this.tultorialController = tultorialController;
            this.repository = repository;
        }

        public async Task OnGuildUserJoin(IDiscordService discord, UserModel user, ulong guildId)
        {
            await discord.SendMessage(AppSettings.chlId_Registro, new MessageModel() { content = $"Bem Vindo a o Servidor. <@{user.id}>" });
            await tultorialController.Process(discord, repository, new MessageModel() { author = user, loc = new O_Vigia_Docker.core.application.models.LocMessage(guildId, 0, 0) });
        }

        public async Task OnGuildUserLeft(IDiscordService discord, UserModel user, ulong guildId)
        {
            await discord.SendMessage(AppSettings.chlId_Registro, new MessageModel() { content = $"Deus, que pena tivemos que nós separar nossas jornadas. <@{user.id}>" });
        }

        public async Task OnMessageCreate(IDiscordService discord, MessageModel msg)
        {
            await textCommandHandler.ProcessCommand(discord, msg);
            await tultorialController.Process(discord, repository, msg);
        }

        public async Task OnMessageEdit(IDiscordService discord, MessageModel msg)
        {
            await textCommandHandler.ProcessCommand(discord, msg);
        }
    }
}
