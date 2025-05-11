using O_Vigia_Docker.core.application.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.application.models
{
    internal class UserTultorialModel
    {
        public ulong userId;
        public EnumTultorialStages tultorialStage = EnumTultorialStages.None;
        public int StageProgress = 0;
        public ulong topicId;
    }
}
