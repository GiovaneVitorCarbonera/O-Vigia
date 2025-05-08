using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.atributos;
using O_Vigia_Docker.core.application.enums;
using O_Vigia_Docker.core.application.handlers;
using O_Vigia_Docker.core.application.models;
using O_Vigia_Docker.core.ports.interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace O_Vigia.core.ports
{
    internal class TextCommandHandler_Reflection : ITextCommandHandler
    {
        private ILogHandler _logHandler;
        private IRepository _repository;
        private ICommandParser _commandParser;
        private ICommandExecutor _commandExecutor;

        public TextCommandHandler_Reflection(ILogHandler logHandler, IRepository repository, ICommandParser commandPaser, ICommandExecutor commandExecutor)
        {
            this._logHandler = logHandler;
            this._repository = repository;
            this._commandParser = commandPaser;
            this._commandExecutor = commandExecutor;
        }

        public async Task ProcessCommand(IDiscordService discord, MessageModel msg)
        {
            if (IsInvalidMessage(msg))
                return;

            string cleanContent = CleanContent(msg.content);
            if (cleanContent == null)
                return;

            await ProcessTextAndExecuteCommands(msg, cleanContent, discord);
        }

        private bool IsInvalidMessage(MessageModel msg)
        {
            return msg == null || msg.loc?.guildId == null || msg.author.isBot;
        }

        private string CleanContent(string content) =>
            content?.Trim();

        private async Task ProcessTextAndExecuteCommands(MessageModel msg, string cleanContent, IDiscordService discord)
        {
            GuildConfigModel? configGuild = await _repository.GetGuildConfig((ulong)msg.loc.guildId);
            string cleanContentForCommands = null;

            var commands = _commandParser.GetAllMethods(_commandParser.GetAllMatchs(configGuild, cleanContent));
            foreach(var command in commands)
            {
                if (string.IsNullOrWhiteSpace(cleanContentForCommands))
                    cleanContentForCommands = await _commandParser.RemoveAllCommandInText(cleanContent, commands.Select(x => x.match).ToList());

                var userPerms = await discord.GetGuildUserPerms((ulong)msg.loc.guildId, msg.author.id);
                if (!userPerms.HasFlag(command.att.reqPerms))
                {
                    await discord.SendMessage(msg.loc.channelId, new MessageModel() { content = $"Sem a Permissão Nessaria. ({command.att.reqPerms.ToString()})", msgReplyId = msg.loc.messageId });
                    return;
                }

                await _commandExecutor.RunCommand(discord, command.mt, command.ty, msg, _commandParser.ExtractArgs(command.commandContent), cleanContentForCommands);
            }

        }
    }
}
