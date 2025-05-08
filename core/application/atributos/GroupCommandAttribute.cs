using O_Vigia_Docker.core.application.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.application.atributos
{
    internal class GroupCommandAttribute : Attribute
    {
        public string prefix { get; private set; }
        public string suffix { get; private set; }
        public bool reqGuildPrefix { get; private set; }

        public GroupCommandAttribute(string prefix = null, string suffix = "\n", bool reqGuildPrefix = true)
        {
            if (prefix == null)
                prefix = "";

            this.prefix = prefix.ToLower();
            this.suffix = suffix.ToLower();
            this.reqGuildPrefix = reqGuildPrefix;
        }

    }
}
