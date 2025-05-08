using O_Vigia_Docker.core.application.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia.core.application.models
{
    internal class MessageModel
    {
        public LocMessage loc;
        public UserModel author;
        public string content;
        public ulong msgReplyId;
        public List<string> imageLinks = new List<string>();
    }
}
