using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.models;
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
            Type type = source.GetType();
            if (type == typeof(IGuildChannel) || type == typeof(Channel))
            {
                IGuildChannel channel = (IGuildChannel)source;
                ChannelModel newChannel = new ChannelModel();
                newChannel.id = channel.Id;
                newChannel.name = channel.Name;
                return newChannel;
            }
            return null;
        }

        public GuildModel ConvertGuild(object source)
        {
            Type type = source.GetType();
            if (type == typeof(Guild) || type == typeof(RestGuild))
            {
                Guild channel = (Guild)source;
                GuildModel newGuild = new GuildModel();
                newGuild.id = channel.Id;
                newGuild.name = channel.Name;
                return newGuild;
            }
            return null;
        }

        public MessageModel ConvertMessage(object source)
        {
            Type type = source.GetType();
            if (type == typeof(Message))
            {
                Message msg = (Message)source;
                MessageModel newMsg = new MessageModel();
                newMsg.content = msg.Content;
                newMsg.author = ConvertUser(msg.Author);
                newMsg.loc = new LocMessage(msg.GuildId, msg.ChannelId, msg.Id);
                if (msg.ReferencedMessage != null)
                    newMsg.msgReplyId = msg.ReferencedMessage.Id;

                return newMsg;
            }
            return null;
        }

        public UserModel ConvertUser(object source)
        {
            Type type = source.GetType();
            if (type == typeof(User) || type == typeof(GuildUser))
            {
                User user = (User)source;
                UserModel newUser = new UserModel();
                newUser.id = user.Id;
                newUser.userName = user.Username;
                newUser.globalName = user.GlobalName;
                return newUser;
            }
            return null;
        }
    }
}
