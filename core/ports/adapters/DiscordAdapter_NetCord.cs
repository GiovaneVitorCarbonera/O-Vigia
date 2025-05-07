using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.ports.adapters
{
    internal class DiscordAdapter_NetCord : IDiscordAdapter
    {
        public ChannelModel ConvertChannel(object source)
        {
            throw new NotImplementedException();
        }

        public GuildModel ConvertGuild(object source)
        {
            throw new NotImplementedException();
        }

        public MessageModel ConvertMessage(object source)
        {
            throw new NotImplementedException();
        }

        public UserModel ConvertUser(object source)
        {
            throw new NotImplementedException();
        }
    }
}
