using O_Vigia_Docker.core.application.atributos;
using O_Vigia_Docker.core.application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.ports.interfaces
{
    internal interface ICommandParser
    {
        List<(MatchCollection match, Type ty)> FindAllCommands(GuildConfigModel configGuild, string content);
        Task<string> RemoveAllCommandInText(string text, List<(MatchCollection match, Type ty)> commands);
        Task<string> RemoveAllCommandInText(string text, ulong guildId);
        string[] ExtractArgs(string msg);
        string GeneratePattern(GroupCommandAttribute groupCmd, string guildPrefix, string content);
        List<(CommandAttribute Attr, MethodInfo Method)> GetMethods(Type ty);
    }
}
