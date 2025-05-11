using O_Vigia.configs;
using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using O_Vigia_Docker.core.application.enums;
using O_Vigia_Docker.core.ports.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.ports
{
    internal class TultorialController_Imerssive : ITultorialController
    {
        public async Task Process(IDiscordService discord, IRepository repo, MessageModel msg)
        {
            if (msg.loc.guildId == null || msg.author.isBot || msg.author.roleIds.Contains(AppSettings.roleid_Verify))
                return;
            var userStages = await repo.GetUserTutorial((ulong)msg.loc.guildId, msg.author.id);

            if (userStages.tultorialStage == EnumTultorialStages.None)
            {
                userStages.topicId = (await discord.CreateTopic(AppSettings.chid_Tultorial, $"{msg.author.id}")).id;
                userStages.tultorialStage = EnumTultorialStages.EmProgress;
                await discord.SendMessage(userStages.topicId, new MessageModel() { content = $"Bem Vindo a o Servidor, esse sera um tultorial apra você aprender o basico para conseguir jogar nesse servidor. {msg.author.GetMention()}\n\nMande uma menssagem aqui para continuarmos." });
                await repo.SetUserTultorial((ulong)msg.loc.guildId, userStages);
                return;
            }
            else if (userStages.tultorialStage == EnumTultorialStages.Completo)
            {
                await discord.DeleteChannel(userStages.topicId);
                await discord.AddRoleForUser((ulong)msg.loc.guildId, msg.author.id, AppSettings.roleid_Verify);
                return;
            }

            if (userStages.tultorialStage != EnumTultorialStages.EmProgress)
                return;

            await discord.SendMessage(userStages.topicId, new MessageModel() { content = $"Stage 1." });
            userStages.tultorialStage = EnumTultorialStages.Completo;
            await repo.SetUserTultorial((ulong)msg.loc.guildId, userStages);
        }
    }
}
