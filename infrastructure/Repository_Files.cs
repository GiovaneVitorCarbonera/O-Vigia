using NetCord;
using NetCord.Gateway;
using Newtonsoft.Json;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace O_Vigia.infrastructure
{
    internal class Repository_Files : IRepository
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        };
        private readonly string _pathBase;

        public Repository_Files()
        {
            _pathBase = Path.Combine(Directory.GetCurrentDirectory(), "data");
        }

        private string getPathGuild(ulong guildId, string[] args)
        {
            List<string> values = new List<string>();
            values.Add(_pathBase);
            values.Add(guildId.ToString());
            values.AddRange(args);
            return Path.Combine(values.ToArray());
        }

        private void checkPath(string path)
        {
            string dirpath = Path.GetDirectoryName(path);

            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
        }

        public async Task<GuildConfigModel?> GetGuildConfig(ulong guildId)
        {
            string path = getPathGuild(guildId, new[] { "guildConfig.json" });
            if (!File.Exists(path))
                return null;

            string jsonText = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<GuildConfigModel>(jsonText, _settings);
        }

        public async Task SetGuildConfig(ulong guildId, GuildConfigModel newConfig)
        {
            string path = getPathGuild(guildId, new[] { "guildConfig.json" });
            checkPath(path);
            File.WriteAllText(path, JsonConvert.SerializeObject(newConfig, _settings));
        }

        public async Task<AvatarModel> GetAvatar(ulong guildId, ulong avatarId)
        {
            string path = getPathGuild(guildId, new[] { "avatares" });
            if (!Directory.Exists(path))
                return null;

            DirectoryInfo dir = new DirectoryInfo(path);
            foreach(var file in dir.GetFiles())
            {
                string name = file.Name.Replace(".json", "");
                var avatares = await GetAllAvatar(guildId, ulong.Parse(name));
                if (avatares == null)
                    continue;

                var avatar = avatares.FirstOrDefault(x => x.id == avatarId);
                if (avatar != null)
                    return avatar;
            }

            return null;
        }

        public async Task<List<AvatarModel>> GetAllAvatar(ulong guildId, ulong userId)
        {
            string path = getPathGuild(guildId, new[] { "avatares", $"{userId}.json" });
            checkPath(path);
            if (!File.Exists(path))
                return null;

            string jsonText = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<AvatarModel>>(jsonText, _settings);
        }

        public async Task AddAvatar(ulong guildId, ulong userId, AvatarModel newAvatar)
        {
            List<AvatarModel> avatares = await GetAllAvatar(guildId, userId);
            if (avatares == null)
                avatares = new List<AvatarModel>();
            avatares.Add(newAvatar);

            string path = getPathGuild(guildId, new[] { "avatares", $"{userId}.json" });
            checkPath(path);
            File.WriteAllText(path, JsonConvert.SerializeObject(avatares, _settings));
        }

        public async Task RemoveAvatar(ulong guildId, ulong userId, ulong avatarId)
        {
            List<AvatarModel> avatares = await GetAllAvatar(guildId, userId);
            if (avatares == null)
                return;

            int index = avatares.FindIndex(x => x.id == avatarId);
            if (index == -1)
                return;

            avatares.RemoveAt(index);

            string path = getPathGuild(guildId, new[] { "avatares", $"{userId}.json" });
            checkPath(path);
            File.WriteAllText(path, JsonConvert.SerializeObject(avatares, _settings));
        }
    }
}
