using O_Vigia_Docker.core.application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.ports.interfaces
{
    internal interface IRepository
    {
        Task<GuildConfigModel?> GetGuildConfig(ulong guildId);
        Task SetGuildConfig(ulong guildId, GuildConfigModel newConfig);
    }
}
