using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.application.atributos
{
    internal class GroupCommandAttribute : Attribute
    {
        public List<string> prefix { get; private set; }
        public List<string> suffix { get; private set; }
        public bool reqGuildPrefix { get; private set; }

        public GroupCommandAttribute(string[] prefix, string[] suffix = null, bool reqGuildConfig = true)
        {
            if (prefix == null)
                prefix = new string[0];
            if (suffix == null)
                suffix = new string[0];

            this.prefix = prefix.Select(x => x.ToLower()).ToList();
            this.suffix = suffix.Select(x => x.ToLower()).ToList();
            this.reqGuildPrefix = reqGuildConfig;
        }

    }
}
