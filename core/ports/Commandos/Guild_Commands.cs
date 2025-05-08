using O_Vigia_Docker.core.application.atributos;
using O_Vigia_Docker.core.application.handlers;
using O_Vigia_Docker.core.application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.ports.Commandos
{
    [GroupCommand("guild")]
    internal class Guild_Commands : CommandHandler
    {
        [Command(new[] { "setup" }, application.enums.EnumPerms.Administrator)]
        public async Task<string> GuildSetup()
        {
            if (_msg.loc.guildId == null)
                return $"E impossivel fazer essa ação.";

            GuildConfigModel guildConfig = await _repository.GetGuildConfig((ulong)_msg.loc.guildId);

            if (guildConfig != null)
                return $"O servidor já foi inicializado.";

            guildConfig = new GuildConfigModel();

            await _repository.SetGuildConfig((ulong)_msg.loc.guildId, guildConfig);
            return $"O servidor foi inicializado.";
        }

        [Command(new[] { "set prefix" }, application.enums.EnumPerms.Administrator)]
        public async Task<string> GuildPrefix()
        {
            if (_msg.loc.guildId == null)
                return $"E impossivel fazer essa ação.";

            if (_args.Length == 0)
                return $"Esta faltando o novo prefix.";

            GuildConfigModel guildConfig = await _repository.GetGuildConfig((ulong)_msg.loc.guildId);

            if (guildConfig == null)
                return $"O servidor não foi inicializado.";

            guildConfig.prefix = _args[0];

            await _repository.SetGuildConfig((ulong)_msg.loc.guildId, guildConfig);
            return $"O Prefix foi atualizado para: \"{_args[0]}\".";
        }
    }
}
