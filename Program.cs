using O_Vigia.configs;
using O_Vigia.core.ports;
using O_Vigia.core.ports.adapters;
using O_Vigia.core.ports.interfaces;
using O_Vigia.infrastructure;
using O_Vigia_Docker.core.ports;
using O_Vigia_Docker.core.ports.interfaces;

ILogHandler logHandler = new LogHandler_Console();
IRepository repository = new Repository_Files();
ITultorialController tultorialController = new TultorialController_Imerssive();

ICommandParser commandParser = new CommandParser_Regex(repository);
ICommandExecutor commandExecutor = new CommandExecuter_Reflection(logHandler, repository);
ITextCommandHandler textCommandHandler = new TextCommandHandler_Reflection(logHandler, repository, commandParser, commandExecutor);

IDiscordHandler discordHandler = new DiscordHandler_CommandBasead(textCommandHandler, repository, tultorialController);
IDiscordAdapter discordAdapter = new DiscordAdapter_NetCord();
IDiscordService discord = new DiscordService_NetCord(logHandler, discordHandler, discordAdapter);

await discord.StartBot(AppSettings.tokenBot);
await Task.Delay(-1);