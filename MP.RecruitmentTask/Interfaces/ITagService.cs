using MP.RecruitmentTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.RecruitmentTask.Interfaces
{
    public interface ITagService
    {
        Task<StackOverflowItem> GetTop1000Tags();
    }
}
