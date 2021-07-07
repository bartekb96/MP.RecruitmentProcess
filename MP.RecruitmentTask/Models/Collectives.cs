using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.RecruitmentTask.Models
{
    public class Collectives
    {
        public string description { get; set; }
        public List<ExternalLink> external_links { get; set; }
        public string link { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public List<string> tags { get; set; }
    }
}
