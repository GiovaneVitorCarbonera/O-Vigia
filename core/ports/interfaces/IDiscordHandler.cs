using O_Vigia.core.application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.ports.interfaces
{
    internal interface IDiscordHandler
    {
        Task OnMessageCreate(IDiscordService discord, MessageModel msg);
        Task OnMessageEdit(IDiscordService discord, MessageModel msg);
        Task OnGuildUserJoin(IDiscordService discord, UserModel user, ulong guildId);
        Task OnGuildUserLeft(IDiscordService discord, UserModel user, ulong guildId);
    }
}
