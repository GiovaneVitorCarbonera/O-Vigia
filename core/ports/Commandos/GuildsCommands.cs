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
    [GroupCommand(new[] { "guild" })]
    internal class GuildsCommands : CommandHandler
    {
        [Command(new[] { "setup" }, application.enums.EnumPerms.Administrator)]
        public async Task<string> GuildSetup()
        {
            if (msg.loc.guildId == null)
                return $"E impossivel fazer essa ação.";

            GuildConfigModel guildConfig = await repository.GetGuildConfig((ulong)msg.loc.guildId);

            if (guildConfig != null)
                return $"O servidor já foi inicializado.";

            guildConfig = new GuildConfigModel();

            await repository.SetGuildConfig((ulong)msg.loc.guildId, guildConfig);
            return $"O servidor foi inicializado.";
        }

        [Command(new[] { "set prefix" }, application.enums.EnumPerms.Administrator)]
        public async Task<string> GuildPrefix()
        {
            if (msg.loc.guildId == null)
                return $"E impossivel fazer essa ação.";

            if (args.Length == 0)
                return $"Esta faltando o novo prefix.";

            GuildConfigModel guildConfig = await repository.GetGuildConfig((ulong)msg.loc.guildId);

            if (guildConfig == null)
                return $"O servidor não foi inicializado.";

            guildConfig.prefix = args[0];

            await repository.SetGuildConfig((ulong)msg.loc.guildId, guildConfig);
            return $"O Prefix foi atualizado para: \"{args[0]}\".";
        }
    }
}
