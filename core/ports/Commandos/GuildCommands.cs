using O_Vigia_Docker.core.application.atributos;
using O_Vigia_Docker.core.application.handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.ports.Commandos
{
    [GroupCommand(new[] { "guild" })]
    internal class GuildCommands : CommandHandler
    {
        [Command(new[] { "ping" })]
        public async Task SendPong()
        {
            await discord.SendMessage(msg.loc.channelId, new O_Vigia.core.application.models.MessageModel() { content = "pong.", msgReplyId = msg.loc.messageId });
        }
    }
}
