using O_Vigia.core.application.models;
using O_Vigia_Docker.core.application.enums;
using O_Vigia_Docker.core.application.models;
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
        IAsyncEnumerable<MessageModel> GetMessages(ulong channelId);
        Task<MessageModel> GetMessage(LocMessage loc);
        Task DeleteMessage(LocMessage loc);
        Task DeleteMessages(ulong channelId, List<ulong> Ids);

        // User
        Task<EnumPerms> GetGuildUserPerms(ulong guildId, ulong userId);

        // Webhook
        Task SendMessageWebHook(WebHookModel webHook, string username, string avatarUrl, string content, List<string> linkAttached);
        Task<List<WebHookModel>> GetAllChannelWebHook(ulong channelId);
        Task<List<WebHookModel>> GetAllSendMessageWebHook(ulong channelId, bool createIfMissing = false);
        Task CreateWeebHook(ulong channelId, string name);
    }
}
