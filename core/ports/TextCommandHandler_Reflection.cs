using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.atributos;
using O_Vigia_Docker.core.application.enums;
using O_Vigia_Docker.core.application.handlers;
using O_Vigia_Docker.core.application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.ports
{
    internal class TextCommandHandler_Reflection : ITextCommandHandler
    {
        private Dictionary<GroupCommandAttribute, Type> _classComand = new Dictionary<GroupCommandAttribute, Type>();
        private Dictionary<CommandAttribute, MethodInfo> _methodComand = new Dictionary<CommandAttribute, MethodInfo>();
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
            if (!CheckMessage(msg))
                return;

            string content = await processContent(msg);
            if (content == null)
                return;

            await _logHandler.AddLog(this.GetType().Name, $"{content}");
            await processCommand(msg, content, discord);
        }

        private bool CheckMessage(MessageModel msg)
        {
            if (msg.loc == null || msg.loc.guildId == null || msg.author.id != 237715069767647234)
                return false;
            return true;
        }

        private async Task<string> processContent(MessageModel msg)
        {
            string content = msg.content.Trim() + " ";

            return content;
        }

        private async Task processCommand(MessageModel msg, string content, IDiscordService discord)
        {
            // Buscar o methodInfo
            GuildConfigModel configGuild = await _repository.GetGuildConfig((ulong)msg.loc.guildId);
            Type cmdType = null;
            MethodInfo cmdMethod = null;
            EnumPerms reqPerms = GettingMethods(configGuild, msg, ref cmdType, ref cmdMethod, content, out string contentFiltred);
            string requiredPermission = null;

            var userPerms = await discord.GetGuildUserPerms((ulong)msg.loc.guildId, msg.author.id);
            if (!userPerms.HasFlag(reqPerms))
            {
                await discord.SendMessage(msg.loc.channelId, new MessageModel() { content = $"Sem a Permissão Nessaria. ({reqPerms.ToString()})", msgReplyId = msg.loc.messageId });
            }
            if (cmdType == null || cmdMethod == null)
                return;

            await RunCommand(msg, contentFiltred, cmdType, cmdMethod, discord);
        }

        private EnumPerms GettingMethods(GuildConfigModel configGuild, MessageModel msg, ref Type cmdType, ref MethodInfo cmdMethod, string content, out string contentOutPrefixAndSuffix)
        {
            string groupPrefix = null;
            string groupSuffix = null;
            string cmdPrefix = null;
            EnumPerms reqPermissions = EnumPerms.None;

            foreach (var classAtt in _classComand)
            {
                // Valid Command
                bool check = false;
                string guildPrefix = null;

                if (classAtt.Key.reqGuildPrefix && configGuild != null)
                    guildPrefix = configGuild.prefix;

                foreach (var prefix in classAtt.Key.prefix)
                {
                    for (int i = 0; i <= classAtt.Key.suffix.Count; i++)
                    {
                        foreach (var mthAtt in _methodComand)
                        {
                            foreach (var mthPrefix in mthAtt.Key.prefix)
                            {
                                if (!content.StartsWith($"{guildPrefix}{prefix} {mthPrefix}".ToLower()))
                                    continue;

                                if (classAtt.Key.suffix.Count > 0)
                                {
                                    string suffix = classAtt.Key.suffix[i];
                                    if (!content.EndsWith($"{suffix}".ToLower()))
                                        continue;
                                    groupSuffix = suffix;
                                }
                                else
                                    groupSuffix = "";

                                reqPermissions = mthAtt.Key.reqPerms;
                                groupPrefix = $"{guildPrefix}{prefix}";
                                cmdPrefix = mthPrefix;
                                cmdType = classAtt.Value;
                                cmdMethod = mthAtt.Value;
                                break;
                            }
                            if (!string.IsNullOrEmpty(groupPrefix))
                                break;
                        }
                        if (!string.IsNullOrEmpty(groupPrefix))
                            break;
                    }
                    if (!string.IsNullOrEmpty(groupPrefix))
                        break;
                }
                if (!string.IsNullOrEmpty(groupPrefix))
                    break;
            }

            string fullPrefix = $"{groupPrefix} {cmdPrefix}";
            string fullSuffix = $"{groupSuffix}";
            contentOutPrefixAndSuffix = content.Substring(fullPrefix.Length, content.Length - fullSuffix.Length - fullPrefix.Length);
            return reqPermissions;
        }

        private List<string> ExtractArgsAspas(ref string msg)
        {
            List<string> argsCustom = new List<string>();
            while (msg.Contains("\""))
            {
                int IndexStart = msg.IndexOf("\"");
                int IndexEnd = msg.IndexOf("\"", IndexStart + 1);

                string text = msg.Substring(IndexStart + 1, IndexEnd - IndexStart - 1);
                msg = msg.Replace($"\"{text}\"", null).Trim();
                argsCustom.Add(text);
            }
            return argsCustom;
        }

        private async Task RunCommand(MessageModel msg, string argsContent, Type typeClass, MethodInfo typeMethod, IDiscordService discord)
        {
            try
            {
                var msgLoc = msg.loc;
                var Args = ExtractArgsAspas(ref argsContent);
                Args.AddRange(argsContent.Split(' ').ToList().FindAll(x => !string.IsNullOrEmpty(x)));

                object instance = Activator.CreateInstance(typeClass);
                if (instance == null || !(instance is CommandHandler)) return;

                CommandHandler CB = (CommandHandler)instance;
                CB.SetArgs(_logHandler, _repository, discord, msg, Args.ToArray());

                await (Task)typeMethod.Invoke(CB, null);
            }
            catch (Exception ex)
            {
                await _logHandler.AddLog(this.GetType().Name, ex.Message, ex);
            }
        }
    }
}
