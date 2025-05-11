using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.application.models
{
    internal class UserModel
    {
        public ulong id;
        public string userName;
        public string globalName;
        public bool isBot;
        public List<ulong> roleIds = new List<ulong>();

        public string GetMention()
        {
            return $"<@{id}>";
        }
    }
}
