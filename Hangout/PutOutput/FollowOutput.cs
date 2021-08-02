using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.PutOutput
{
    public class FollowOutput
    {
        public FollowOutput(List<int> followeds, List<int> followers)
        {
            this.followeds = followeds;
            this.followers = followers;
        }

        public List<int> followeds { get; set; }

        public List<int> followers { get; set; }

    }
}
