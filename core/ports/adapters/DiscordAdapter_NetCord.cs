using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.enums;
using O_Vigia_Docker.core.application.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
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
            if (type == typeof(RestMessage) || type == typeof(Message))
            {
                RestMessage msg = (RestMessage)source;
                MessageModel newMsg = new MessageModel();
                newMsg.content = msg.Content;
                newMsg.author = ConvertUser(msg.Author);
                newMsg.loc = new LocMessage(null, msg.ChannelId, msg.Id);
                if (type == typeof(Message))
                    newMsg.loc.guildId = ((Message)source).GuildId;
                if (msg.ReferencedMessage != null)
                    newMsg.msgReplyId = msg.ReferencedMessage.Id;
                newMsg.imageLinks = msg.Attachments.Where(x => x.ContentType != null && x.ContentType.Contains("image")).Select(x => x.Url).ToList();

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
                newUser.isBot = user.IsBot;
                return newUser;
            }
            return null;
        }

        public WebHookModel ConvertWebHook(object source)
        {
            Type type = source.GetType();
            if (type == typeof(Webhook) || type == typeof(IncomingWebhook))
            {
                Webhook web = (Webhook)source;
                WebHookModel newWeb = new WebHookModel();
                newWeb.id = web.Id;
                newWeb.name = web.Name;
                newWeb.url = web.Url;
                newWeb.type = (EnumWebHookType)web.Type;

                if (web.Type == WebhookType.Incoming)
                    newWeb.token = (web as IncomingWebhook).Token;

                return newWeb;
            }
            return null;
        }
    }
}
