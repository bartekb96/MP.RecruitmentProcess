using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.RecruitmentTask.Models
{
    public class StackOverflowItem
    {
        public List<Tag> items { get; set; }
        public bool has_more { get; set; }
        public int quota_max { get; set; }
        public int quota_remaining { get; set; }
    }
}
