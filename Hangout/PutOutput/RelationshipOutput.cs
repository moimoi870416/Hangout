using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.PutOutput
{
    public class RelationshipOutput
    {
        public RelationshipOutput(int protagonistId, int times)
        {
            ProtagonistId = protagonistId;
            Times = times;
        }

        public int ProtagonistId { get; set; }
        public int Times { get; set; }

    }
}
