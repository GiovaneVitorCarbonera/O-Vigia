using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.application.models
{
    internal class LocMessage
    {
        public ulong? guildId;
        public ulong channelId;
        public ulong messageId;

        public LocMessage(ulong? guildId, ulong channelId, ulong messageId)
        {
            this.guildId = guildId;
            this.channelId = channelId;
            this.messageId = messageId;
        }
    }
}
