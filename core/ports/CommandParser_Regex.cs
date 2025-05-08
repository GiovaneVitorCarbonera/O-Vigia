using NetCord.Gateway;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.atributos;
using O_Vigia_Docker.core.application.models;
using O_Vigia_Docker.core.ports.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace O_Vigia_Docker.core.ports
{
    internal class CommandParser_Regex : ICommandParser
    {
        // Definições
        record CommandGroup(GroupCommandAttribute Attr, List<(CommandAttribute Attr, MethodInfo Method)> Methods);

        private Dictionary<Type, CommandGroup> _commands = new();
        IRepository _repository;

        public CommandParser_Regex(IRepository repository)
        {
            _repository = repository;
            Setup();
        }

        private void Setup()
        {
            _commands.Clear();

            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<GroupCommandAttribute>() != null);

            foreach (var type in types)
            {
                var groupAttr = type.GetCustomAttribute<GroupCommandAttribute>();
                var methods = type.GetMethods()
                    .Where(m => m.GetCustomAttribute<CommandAttribute>() != null)
                    .Select(m => (m.GetCustomAttribute<CommandAttribute>(), m))
                    .ToList();

                _commands[type] = new CommandGroup(groupAttr, methods);
            }
        }

        public string[] ExtractArgs(string msg)
        {
            var args = new List<string>();
            var matches = Regex.Matches(msg, "\"(.*?)\"");

            foreach (Match match in matches)
                args.Add(match.Groups[1].Value);

            // Remove tudo que foi entre aspas
            msg = Regex.Replace(msg, "\"(.*?)\"", "").Trim();

            // Adiciona o resto (fora das aspas)
            args.AddRange(msg.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            return args.ToArray();
        }

        public List<(MatchCollection match, Type ty)> FindAllCommands(GuildConfigModel configGuild, string content)
        {
            List<(MatchCollection match, Type classAtt)> matchs = new List<(MatchCollection match, Type classAtt)>();
            foreach (var classAtt in _commands)
            {
                string patternRegex = GeneratePattern(classAtt.Value.Attr, configGuild.prefix, content);
                MatchCollection matches = Regex.Matches(content, patternRegex);
                if (matches.Count == 0)
                    continue;

                matchs.Add((matches, classAtt.Key));
            }
            return matchs;
        }

        public string GeneratePattern(GroupCommandAttribute groupCmd, string guildPrefix, string content)
        {
            string prefix = groupCmd.prefix;
            string suffix = groupCmd.suffix == "\n" ? @"(\r?\n|$)" : Regex.Escape(groupCmd.suffix);
            if (groupCmd.reqGuildPrefix)
                prefix = $"{guildPrefix}{prefix}";
            prefix = Regex.Escape(prefix);
            return $"{prefix}\\s*(.*?)\\s*{suffix}";
        }

        public async Task<string> RemoveAllCommandInText(string text, List<(MatchCollection match, Type ty)> commands)
        {
            foreach (var command in commands)
            {
                foreach (Match match in command.match)
                {
                    text = text.Replace(match.Value, "");
                }
            }
            return text;
        }

        public async Task<string> RemoveAllCommandInText(string text, ulong guildId)
        {
            GuildConfigModel? configGuild = await _repository.GetGuildConfig(guildId);
            var commands = FindAllCommands(configGuild, text);

            return await RemoveAllCommandInText(text, commands);
        }

        public List<(CommandAttribute Attr, MethodInfo Method)> GetMethods(Type ty)
        {
            return _commands[ty].Methods;
        }
    }
}
