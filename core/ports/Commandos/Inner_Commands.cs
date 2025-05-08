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
    internal class Inner_Commands : CommandHandler
    {
        [Command(new[] { "avatar" })]
        public async Task<string> SendAvatarMsg()
        {
            List<WebHookModel> webs = await _discord.GetAllSendMessageWebHook(_msg.loc.channelId, true);
            if (webs.Count == 0)
                return "WeebHook esta indisponivel para o envio do avatar.";

            var avatares = await _repository.GetAllAvatar((ulong)_msg.loc.guildId, _msg.author.id);
            if (avatares == null)
                return $"Você não possui nenhum avatar registrado.";

            var avatar = avatares.Find(x => x.nickname == _args[0]);
            if (avatar == null)
                return $"O Avatar com o apelido: ``{_args[0]}`` não existe.";

            string content = _cleandContent;

            if (_msg.msgReplyId != 0)
            {
                var msgReply = await _discord.GetMessage(new LocMessage(_msg.loc.guildId, _msg.loc.channelId, _msg.msgReplyId));
                content = $"> {msgReply.author.GetMention()}\n> {StringUtils.maxStringSize(msgReply.content, 250)}\n{content}";
            }

            await _discord.SendMessageWebHook(webs[0], avatar.username, avatar.avatarImageURL, content, _msg.imageLinks);
            await _discord.DeleteMessage(_msg.loc);
            return null;
        }
    }
}
