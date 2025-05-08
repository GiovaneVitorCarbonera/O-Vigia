using O_Vigia_Docker.core.application.atributos;
using O_Vigia_Docker.core.application.handlers;
using O_Vigia_Docker.core.application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vigia.core.application.utils;

namespace O_Vigia_Docker.core.ports.Commandos
{
    [GroupCommand("[", "]", false)]
    internal class InnerCommands : CommandHandler
    {
        [Command(new[] { "avatar" })]
        public async Task<string> SendAvatarMsg()
        {
            List<WebHookModel> webs = await _discord.GetAllSendMessageWebHook(_msg.loc.channelId, true);
            if (webs.Count == 0)
                return "WeebHook esta indisponivel para o envio do avatar.";
            string content = _cleandContent;

            if (_msg.msgReplyId != 0)
            {
                var msgReply = await _discord.GetMessage(new LocMessage(_msg.loc.guildId, _msg.loc.channelId, _msg.msgReplyId));
                content = $"> {msgReply.author.GetMention()}\n> {StringUtils.maxStringSize(msgReply.content, 250)}\n{content}";
            }

            await _discord.SendMessageWebHook(webs[0], _args[0], null, content);
            await _discord.DeleteMessage(_msg.loc);
            return null;
        }
    }
}
