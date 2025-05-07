using O_Vigia.configs;
using O_Vigia.core.ports;
using O_Vigia.core.ports.adapters;
using O_Vigia.core.ports.interfaces;
using O_Vigia.infrastructure;

ILogHandler logHandler = new LogHandler_Console();
IRepository repository = new Repository_Files();
ITextCommandHandler textCommandHandler = new TextCommandHandler_Reflection(logHandler, repository);

IDiscordHandler discordHandler = new DiscordHandler_CommandBasead(textCommandHandler);
IDiscordAdapter discordAdapter = new DiscordAdapter_NetCord();
IDiscordService discord = new DiscordService_NetCord(logHandler, discordHandler, discordAdapter);

await discord.StartBot(AppSettings.tokenBot);