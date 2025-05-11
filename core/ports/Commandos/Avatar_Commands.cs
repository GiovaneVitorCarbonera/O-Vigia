using O_Vigia_Docker.core.application.atributos;
using O_Vigia_Docker.core.application.handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vigia.core.application.utils;

namespace O_Vigia_Docker.core.ports.Commandos
{
    [GroupCommand("avatar")]
    internal class Avatar_Commands : CommandHandler
    {
        [Command(new[] { "criar", "create" })]
        public async Task<string> CreateAvatar()
        {
            if (_args.Length < 2)
                return "não foi passado o nome do avatar nem o apelido para ele.\n.avatar criar \"{nome}\" {apelido}";

            string avatar_username = StringUtils.convertToSafeString(_args[0]);
            string avatar_nickname = StringUtils.convertToSafeString(_args[1]);
            string? avatar_imagem = _msg.imageLinks.FirstOrDefault();

            if (avatar_username.ToLower() == "o vigia")
                return "Não é permitido esse username.";

            await _repository.AddAvatar((ulong)_msg.loc.guildId, _msg.author.id, new application.models.AvatarModel()
            {
                nickname = avatar_nickname,
                owner_userId = _msg.author.id,
                username = avatar_username,
                avatarImageURL = avatar_imagem,
            });

            return $"O avatar foi criado com sucesso, do apelido: \"{avatar_nickname}\".";
        }

        [Command(new[] { "remover", "deletar" })]
        public async Task<string> RemoveAvatar()
        {
            if (_args.Length < 1)
                return "não foi passado o apelido para o avatar.\n.avatar criar {apelido}";

            string avatar_nickname = _args[0];

            var avatares = await _repository.GetAllAvatar((ulong)_msg.loc.guildId, _msg.author.id);
            if (avatares == null)
                return "não foi encontrado nenhum avatar registrado nesse usuario.";

            var avatar = avatares.Find(x => x.nickname == avatar_nickname);
            if (avatar == null)
                return "não foi encontrado o avatar do apelido nesse usuario.";

            await _repository.RemoveAvatar((ulong)_msg.loc.guildId, _msg.author.id, avatar.id);
            return $"O avatar foi deletado com sucesso, do apelido: \"{avatar_nickname}\".";
        }

        [Command(new[] { "imagem" })]
        public async Task<string> UpdateImage()
        {
            if (_args.Length < 1)
                return "Não foi passado o apelido o apelido do avatar.";

            if (_msg.imageLinks.Count <= 0)
                return "E nessesario anexar uma imagem pra poder atualizar a imagem do personagem.";

            string avatar_nickname = _args[0];

            var avatares = await _repository.GetAllAvatar((ulong)_msg.loc.guildId, _msg.author.id);
            if (avatares == null)
                return "não foi encontrado nenhum avatar registrado nesse usuario.";

            var avatar = avatares.Find(x => x.nickname == avatar_nickname);
            if (avatar == null)
                return "não foi encontrado o avatar do apelido nesse usuario.";

            avatar.avatarImageURL = _msg.imageLinks[0];
            await _repository.RemoveAvatar((ulong)_msg.loc.guildId, _msg.author.id, avatar.id);
            await _repository.AddAvatar((ulong)_msg.loc.guildId, _msg.author.id, avatar);
            return $"O avatar foi atualizado com sucesso, do apelido: \"{avatar_nickname}\".";
        }

        [Command(new[] { "lista" })]
        public async Task<string> ListaDeAvatares()
        {
            var avatares = await _repository.GetAllAvatar((ulong)_msg.loc.guildId, _msg.author.id);
            if (avatares == null)
                return "não foi encontrado nenhum avatar registrado nesse usuario.";

            return $"> Avatares:\n{string.Join("\n", avatares.Select(x => $"[{x.nickname}] {x.username}"))}";
        }
    }
}
