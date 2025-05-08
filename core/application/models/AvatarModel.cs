using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.application.models
{
    internal class AvatarModel
    {
        public ulong id;
        public ulong owner_userId;
        public string nickname;
        public string username;
        public string avatarImageURL;
    }
}
