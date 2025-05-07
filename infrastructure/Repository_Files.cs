using Newtonsoft.Json;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _pathBase = $"{Directory.GetCurrentDirectory()}\\data";
        }

        private string getPathGuild(ulong guildId, string name)
        {
            return $"{_pathBase}\\{guildId}\\{name}";
        }

        private void checkPath(string path)
        {
            string dirpath = Path.GetDirectoryName(path);

            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
        }

        public async Task<GuildConfigModel> GetGuildConfig(ulong guildId)
        {
            string path = getPathGuild(guildId, "guildConfig.json");
            if (!File.Exists(path))
                return null;

            string jsonText = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<GuildConfigModel>(jsonText, _settings);
        }

        public async Task SetGuildConfig(ulong guildId, GuildConfigModel newConfig)
        {
            string path = getPathGuild(guildId, "guildConfig.json");
            checkPath(path);
            File.WriteAllText(path, JsonConvert.SerializeObject(newConfig, _settings));
        }
    }
}
