using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.atributos;
using O_Vigia_Docker.core.application.enums;
using O_Vigia_Docker.core.application.handlers;
using O_Vigia_Docker.core.application.models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.ports
{
    internal class TextCommandHandler_Reflection : ITextCommandHandler
    {
        private Dictionary<GroupCommandAttribute?, Type> _classComand = new Dictionary<GroupCommandAttribute?, Type>();
        private Dictionary<CommandAttribute?, MethodInfo> _methodComand = new Dictionary<CommandAttribute?, MethodInfo>();
        private ILogHandler _logHandler;
        private IRepository _repository;

        public TextCommandHandler_Reflection(ILogHandler logHandler, IRepository repository)
        {
            this._logHandler = logHandler;
            this._repository = repository;
            Setup();
        }

        private void Setup()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.GetCustomAttribute<GroupCommandAttribute>() != null)
                .ToList();

            _classComand.Clear();
            _methodComand.Clear();
            foreach (var type in types)
            {
                _classComand.Add(type.GetCustomAttribute<GroupCommandAttribute>(), type);

                var mths = type.GetMethods()
                        .Where(x => x.GetCustomAttribute<CommandAttribute>() != null)
                        .ToList();

                foreach (var method in mths)
                {
                    _methodComand.Add(method.GetCustomAttribute<CommandAttribute>(), method);
                }
            }
        }

        public async Task ProcessCommand(IDiscordService discord, MessageModel msg)
        {
            if (!IsValidMessage(msg))
                return;

            string cleanContent = CleanContent(msg.content);
            if (cleanContent == null)
                return;

            await ProcessCommand(msg, cleanContent, discord);
        }

        private bool IsValidMessage(MessageModel msg) =>
            msg.loc?.guildId != null;

        private string CleanContent(string content) =>
            content?.Trim();

        private async Task ProcessCommand(MessageModel msg, string content, IDiscordService discord)
        {
            GuildConfigModel configGuild = await _repository.GetGuildConfig((ulong)msg.loc.guildId);
            // Buscar o Type e MethodInfo
            EnumPerms reqPerms = GettingMethods(configGuild, msg, out Type cmdType, out MethodInfo cmdMethod, content, out string contentFiltred);
            
            if (cmdType == null || cmdMethod == null)
                return;
            // Verificar se tem Permissão
            var userPerms = await discord.GetGuildUserPerms((ulong)msg.loc.guildId, msg.author.id);
            if (!userPerms.HasFlag(reqPerms))
            {
                await discord.SendMessage(msg.loc.channelId, new MessageModel() { content = $"Sem a Permissão Nessaria. ({reqPerms.ToString()})", msgReplyId = msg.loc.messageId });
                return;
            }
            

            // Loga e Tenta executar o commando
            await _logHandler.AddLog(this.GetType().Name, $"Commando: \"{msg.content}\"");
            await RunCommand(msg, contentFiltred, cmdType, cmdMethod, discord);
        }

        private EnumPerms GettingMethods(GuildConfigModel configGuild, MessageModel msg, out Type cmdType, out MethodInfo cmdMethod, string content, out string contentOutPrefixAndSuffix)
        {
            EnumPerms reqPermissions = EnumPerms.None;
            contentOutPrefixAndSuffix = null;

            foreach (var classAtt in _classComand)
            {
                if (classAtt.Key == null) continue;
                string guildPrefix = classAtt.Key.reqGuildPrefix && configGuild != null ? configGuild.prefix : "";
                string groupPrefix = "";
                string groupSuffix = "";
                string cmdPrefix = "";

                var methods = _methodComand.Where(x => x.Value.ReflectedType == classAtt.Value).ToList();

                foreach (var prefix in classAtt.Key.prefix.DefaultIfEmpty(""))
                    foreach (var suffix in classAtt.Key.suffix.DefaultIfEmpty(""))
                        foreach (var mthAtt in methods)
                        {
                            if (mthAtt.Key == null) continue;
                            foreach (var mthPrefix in mthAtt.Key.prefix)
                            {
                                string fullCmd = !string.IsNullOrEmpty(prefix) && classAtt.Key.reqGuildPrefix ?
                                    $"{guildPrefix}{prefix} {mthPrefix}".ToLower() :
                                    $"{prefix}{mthPrefix}".ToLower();
                                if (!content.StartsWith(fullCmd))
                                    continue;

                                if (!string.IsNullOrEmpty(suffix) && !content.EndsWith(suffix.ToLower())) continue;

                                groupPrefix = $"{guildPrefix}{prefix}";
                                groupSuffix = suffix;
                                cmdPrefix = mthPrefix;
                                reqPermissions = mthAtt.Key.reqPerms;
                                cmdType = classAtt.Value;
                                cmdMethod = mthAtt.Value;

                                contentOutPrefixAndSuffix = content.Substring(fullCmd.Length, content.Length - fullCmd.Length - groupSuffix.Length);
                                return reqPermissions;
                            }
                        }
                            
            }

            cmdType = null;
            cmdMethod = null;
            return reqPermissions;
        }

        private async Task RunCommand(MessageModel msg, string argsContent, Type typeClass, MethodInfo typeMethod, IDiscordService discord)
        {
            try
            {
                var Args = ExtractArgs(ref argsContent);
                object instance = Activator.CreateInstance(typeClass);

                if (instance == null || !(instance is CommandHandler)) return;

                CommandHandler CB = (CommandHandler)instance;
                CB.SetArgs(_logHandler, _repository, discord, msg, Args.ToArray());

                string rsmsg = await (Task<string>)typeMethod.Invoke(CB, null);

                if (!string.IsNullOrWhiteSpace(rsmsg))
                {
                    await discord.SendMessage(msg.loc.channelId, new O_Vigia.core.application.models.MessageModel()
                    {
                        content = rsmsg,
                        msgReplyId = msg.loc.messageId,
                    });
                }
            }
            catch (Exception ex)
            {
                await _logHandler.AddLog(this.GetType().Name, ex.Message, ex);
            }
        }

        private string[] ExtractArgs(ref string msg)
        {
            var args = new List<string>();
            while (msg.Contains("\""))
            {
                int start = msg.IndexOf("\""), end = msg.IndexOf("\"", start + 1);
                if (end == -1) break;

                args.Add(msg.Substring(start + 1, end - start - 1));
                msg = msg.Remove(start, end - start + 1).Trim();
            }
            args.AddRange(msg.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            return args.ToArray();
        }
    }
}
