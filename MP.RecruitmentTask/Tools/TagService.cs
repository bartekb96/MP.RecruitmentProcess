using MP.RecruitmentTask.Interfaces;
using MP.RecruitmentTask.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MP.RecruitmentTask.Tools
{
    public class TagService : ITagService
    {
        private DateTime LastReadDateTime;

        private StackOverflowItem item;

        public TagService()
        {
            LastReadDateTime = DateTime.Now.AddMinutes(-11);
            item = new StackOverflowItem();
        }

        public async Task<StackOverflowItem> GetTop1000Tags()
        {

            //zapisuje sobie dane co 10 minut żeby nie wykonywać każdorazowo nadmiernych zapytań
            if (LastReadDateTime != null && LastReadDateTime.AddMinutes(10) <= DateTime.Now)
            {

                HttpClientHandler handler = new HttpClientHandler();
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                using (var Client = new HttpClient(handler))
                {
                    Client.BaseAddress = new Uri("https://api.stackexchange.com/");
                    Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));

                    for (int i = 1; i < 11; i++)
                    {
                        var response = Client.GetAsync($"2.3/tags?page={i}&pagesize=100&order=desc&sort=popular&site=stackoverflow");
                        response.Wait();
                        var requestResult = response.Result;

                        if (requestResult.IsSuccessStatusCode)
                        {
                            if (i == 1)
                            {
                                var responseString = await requestResult.Content.ReadAsStringAsync();

                                item = JsonConvert.DeserializeObject<StackOverflowItem>(responseString);
                            }
                            else
                            {
                                var responseString = await requestResult.Content.ReadAsStringAsync();

                                var tempItem = JsonConvert.DeserializeObject<StackOverflowItem>(responseString);

                                item.items.AddRange(tempItem.items);
                            }
                        }
                    }                   
                } 
                
                LastReadDateTime = DateTime.Now;
            }

            return item;
        }

    }
}
