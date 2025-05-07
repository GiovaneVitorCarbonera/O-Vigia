using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
