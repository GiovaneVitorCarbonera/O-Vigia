using O_Vigia.core.application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.ports.interfaces
{
    internal interface ITextCommandHandler
    {
        Task ProcessCommand(IDiscordService discord, MessageModel msg);
    }
}
