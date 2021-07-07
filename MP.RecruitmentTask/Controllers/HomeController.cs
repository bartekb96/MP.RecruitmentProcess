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

namespace MP.RecruitmentTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 100)
        {
            StackOverflowItem item = new StackOverflowItem();

            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                using (var Client = new HttpClient(handler))
                {
                    Client.BaseAddress = new Uri("https://api.stackexchange.com/");
                    Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));

                    //wybieram po 100 rekordów z api do maksymalnie 10 stron (ograniczenie paginacji to 1000 elementów więc 1000/100 = 10)
                    var response = Client.GetAsync($"2.3/tags?page={pageNumber}&pagesize={pageSize}&order=desc&sort=popular&site=stackoverflow"); 
                    response.Wait();
                    var requestResult = response.Result;

                    if (requestResult.IsSuccessStatusCode)
                    {
                        var responseString = await requestResult.Content.ReadAsStringAsync();

                        item = JsonConvert.DeserializeObject<StackOverflowItem>(responseString);

                        int sum = item.items.Sum(t => t.count);

                        item.items.ForEach(i => i.percentageOccur = (double)i.count / (double)sum);                     
                    }
                }
            }
            catch(Exception ex)
            {

            }

            var result = new PagedResult<Tag>
            {
                Data = item.items,
                TotalItems = 1000,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return View("Index", result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
