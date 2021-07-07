using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MP.RecruitmentTask.Models
{
    public class Tag
    {
        public List<Collectives> collectives { get; set; }
        public int count { get; set; }
        public bool has_synonyms { get; set; }
        public bool is_moderator_only { get; set; }
        public bool is_required { get; set; }
        public DateTime? last_activity_date { get; set; }
        public string name { get; set; }
        public List<string> synonyms { get; set; }
        public int? user_id { get; set; }

        [JsonIgnore]
        public double percentageOccur { get; set; }
    }
}
