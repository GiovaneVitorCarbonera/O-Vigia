using O_Vigia.core.application.models;
using O_Vigia_Docker.core.application.atributos;
using O_Vigia_Docker.core.application.handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.ports.Commandos
{
    [GroupCommand(null)]
    internal class ModCommands : CommandHandler
    {
        [Command(new[] { "clear" }, application.enums.EnumPerms.ManageMessages)]
        public async Task<string> clearMsgs()
        {
            if (_args.Length == 0)
                return "E nessesario informa o numero de menssages que serão apagadas.";

            int msgsCounter = Math.Min(int.Parse(_args[0]), 1000) + 1;
            int counter = 0;

            List<ulong> idMessages = new List<ulong>();
            await foreach (MessageModel msg in _discord.GetMessages(_msg.loc.channelId))
            {
                if (counter >= msgsCounter)
                    break;

                idMessages.Add(msg.loc.messageId);
                counter++;
            }
            await _discord.DeleteMessages(_msg.loc.channelId, idMessages);

            return $"Foi apagado com sucesso {counter} menssages.";
        }
    }
}
