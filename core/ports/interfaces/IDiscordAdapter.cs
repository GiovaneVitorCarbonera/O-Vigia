using O_Vigia.core.application.models;
using O_Vigia_Docker.core.application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.ports.interfaces
{
    internal interface IDiscordAdapter
    {
        UserModel ConvertUser(object source);
        MessageModel ConvertMessage(object source);
        ChannelModel ConvertChannel(object source);
        GuildModel ConvertGuild(object source);
        WebHookModel ConvertWebHook(object source);
    }
}
