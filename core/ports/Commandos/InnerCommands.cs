using O_Vigia_Docker.core.application.atributos;
using O_Vigia_Docker.core.application.handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.ports.Commandos
{
    [GroupCommand("[", "]", false)]
    internal class InnerCommands : CommandHandler
    {
        [Command(new[] { "ping" })]
        public async Task<string> SendPong()
        {
            return "pong.";
        }
    }
}
