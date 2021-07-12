using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MP.RecruitmentTask.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using cloudscribe.Pagination.Models;
using MP.RecruitmentTask.Interfaces;

namespace MP.RecruitmentTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ITagService _tagService;
       
        public HomeController(ILogger<HomeController> logger, ITagService tagService)
        {
            _logger = logger;
            _tagService = tagService;
        } 

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 100)
        {
            var item = await _tagService.GetTop1000Tags();

            int sum = item.items.Sum(t => t.count);

            item.items.ForEach(i => i.percentageOccur = (double)i.count / (double)sum);

            var showItems = item.items.Skip((pageNumber-1)* pageSize).Take(pageSize).ToList();

            var result = new PagedResult<Tag>
            {
                Data = showItems,
                TotalItems = item.items.Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View("Index", result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
