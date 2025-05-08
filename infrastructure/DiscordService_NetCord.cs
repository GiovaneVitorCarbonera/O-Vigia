using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.enums;
using O_Vigia_Docker.core.application.models;
using O_Vigia_Docker.core.application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace O_Vigia.infrastructure
{
    internal class DiscordService_NetCord : IDiscordService
    {
        private GatewayClient _client = null;
        private bool _isStarted;
        private readonly ILogHandler _logHandler;
        private readonly IDiscordHandler _discordHandler;
        private readonly IDiscordAdapter _discordAdapter;

        public DiscordService_NetCord(ILogHandler logHandler, IDiscordHandler discordHandler, IDiscordAdapter discordAdapter)
        {
            _logHandler = logHandler;
            _discordHandler = discordHandler;
            _discordAdapter = discordAdapter;
            _isStarted = false;
        }

        public async Task StartBot(string tokenBot)
        {
            if (string.IsNullOrEmpty(tokenBot))
            {
                await _logHandler.AddLog(this.GetType().Name, "O Token do Bot não foi configurado.");
                return;
            }

            _client = new(new BotToken(tokenBot), new GatewayClientConfiguration()
            {
                Intents = GatewayIntents.GuildMessages | GatewayIntents.DirectMessages | GatewayIntents.MessageContent,
            });

            _client.Log += async (log) => { await _logHandler.AddLog(this.GetType().Name, log.Message, log.Exception); };
            _client.Ready += async (log) => { _isStarted = true; await _logHandler.AddLog(this.GetType().Name, "Bot esta Ativo"); };
            _client.MessageCreate += async (msg) => { await _discordHandler.OnMessageCreate(this, _discordAdapter.ConvertMessage(msg)); };
            _client.MessageUpdate += async (msg) => { await _discordHandler.OnMessageEdit(this, _discordAdapter.ConvertMessage(msg)); };

            await _client.StartAsync();

            while (!_isStarted)
            {
                await Task.Delay(500);
            }
        }

        // Actions

        public ulong GetBotId()
        {
            return _client.Id;
        }

        public async Task SendMessage(ulong channelId, MessageModel msg)
        {
            MessageProperties msgConfig = new MessageProperties();
            msgConfig.Content = msg.content;
            if (msg.msgReplyId != 0)
                msgConfig.MessageReference = MessageReferenceProperties.Reply(msg.msgReplyId, false);

            await _client.Rest.SendMessageAsync(channelId, msgConfig);
        }

        public async Task<EnumPerms> GetGuildUserPerms(ulong guildId, ulong userId)
        {
            RestGuild guild = await _client.Rest.GetGuildAsync(guildId);
            GuildUser user = await _client.Rest.GetGuildUserAsync(guildId, userId);
            EnumPerms perms = (EnumPerms)user.GetPermissions(guild);
            perms |= EnumPerms.None;
            return perms;
        }

        public async Task<MessageModel> GetMessage(LocMessage loc)
        {
            var msg = await _client.Rest.GetMessageAsync(loc.channelId, loc.messageId);
            var rsMsg = _discordAdapter.ConvertMessage(msg);
            return rsMsg;
        }

        public async IAsyncEnumerable<MessageModel> GetMessages(ulong channelId)
        {
            await foreach (var msg in _client.Rest.GetMessagesAsync(channelId))
            {
                yield return _discordAdapter.ConvertMessage(msg);
            }
        }

        public async Task DeleteMessage(LocMessage loc)
        {
            await _client.Rest.DeleteMessageAsync(loc.channelId, loc.messageId);
        }

        public async Task DeleteMessages(ulong channelId, List<ulong> Ids)
        {
            await _client.Rest.DeleteMessagesAsync(channelId, Ids);
        }

        public async Task<List<WebHookModel>> GetAllChannelWebHook(ulong channelId)
        {
            var restRs = await _client.Rest.GetChannelWebhooksAsync(channelId);
            List<Webhook> webHook = restRs.ToList();
            return webHook.ConvertAll(x => _discordAdapter.ConvertWebHook(x)).Where(x => x != null).ToList();
        }

        public async Task<List<WebHookModel>> GetAllSendMessageWebHook(ulong channelId, bool createIfMissing = false)
        {
            List<WebHookModel> webs = new List<WebHookModel>();

            while (webs.Count == 0)
            {
                webs = (await GetAllChannelWebHook(channelId)).FindAll(x => x.token != null); ;
                if (webs.Count == 0)
                {
                    await CreateWeebHook(channelId, GetBotId().ToString());
                }
            }

            return webs;
        }

        public async Task SendMessageWebHook(WebHookModel webHook, string username, string avatarUrl, string content, List<string> linkAttached)
        {
            WebhookMessageProperties properties = new WebhookMessageProperties();
            properties.Content = content;
            properties.Username = username;
            properties.AvatarUrl = avatarUrl;

            foreach(string link in linkAttached)
            {
                properties.AddAttachments(new AttachmentProperties(link.Split('/').Last().Split('?')[0], await NetBasic.GetStreamFromUrl(link)));
            }

            await _client.Rest.ExecuteWebhookAsync(webHook.id, webHook.token, properties);
        }

        public async Task CreateWeebHook(ulong channelId, string name)
        {
            await _client.Rest.CreateWebhookAsync(channelId, new WebhookProperties(name));
        }
    }
}
