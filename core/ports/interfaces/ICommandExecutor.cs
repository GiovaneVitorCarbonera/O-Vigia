using O_Vigia.core.application.models;
using O_Vigia.core.ports.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.ports.interfaces
{
    internal interface ICommandExecutor
    {
        Task RunCommand(IDiscordService discord, MethodInfo mth, Type ty, MessageModel msg, string[] args, string cleanContentForCommands);
    }
}
