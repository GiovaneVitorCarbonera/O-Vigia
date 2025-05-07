using O_Vigia_Docker.core.application.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.application.atributos
{
    internal class CommandAttribute : Attribute
    {
        public List<string> prefix { get; private set; }
        public EnumPerms reqPerms { get; private set; }

        public CommandAttribute(string[] prefix, EnumPerms reqPerms = EnumPerms.None)
        {
            if (prefix == null)
                prefix = new string[0];

            this.prefix = prefix.Select(x => x.ToLower()).ToList();
            this.reqPerms = reqPerms;
        }

    }
}
