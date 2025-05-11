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
        // Guild Config Manipulators
        Task<GuildConfigModel?> GetGuildConfig(ulong guildId);
        Task SetGuildConfig(ulong guildId, GuildConfigModel newConfig);


        // Avatar Manipulators
        Task<AvatarModel> GetAvatar(ulong guildId, ulong avatarId);
        Task<List<AvatarModel>> GetAllAvatar(ulong guildId, ulong userId);
        Task AddAvatar(ulong guildId, ulong userId, AvatarModel newAvatar);
        Task RemoveAvatar(ulong guildId, ulong userId, ulong avatarId);


        // User Tultorial Manipulators
        Task<UserTultorialModel> GetUserTutorial(ulong guildId, ulong userId);
        Task SetUserTultorial(ulong guildId, UserTultorialModel newData);
    }
}
