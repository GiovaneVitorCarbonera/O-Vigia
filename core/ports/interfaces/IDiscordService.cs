using O_Vigia.core.application.models;
using O_Vigia_Docker.core.application.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.ports.interfaces
{
    internal interface IDiscordService
    {
        Task StartBot(string tokenBot);

        // Messages
        Task SendMessage(ulong channelId, MessageModel msg);

        // User
        Task<EnumPerms> GetGuildUserPerms(ulong guildId, ulong userId);
    }
}
