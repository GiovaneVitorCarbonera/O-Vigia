using O_Vigia_Docker.core.application.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.application.models
{
    internal class WebHookModel
    {
        public ulong id { get; set; }
        public string token { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public EnumWebHookType type { get; set; }
    }
}
