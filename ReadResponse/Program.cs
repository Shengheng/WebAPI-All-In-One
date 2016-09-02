using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Net.Http.Headers;

namespace ReadResponse
{
    class Program
    {
        static int count = 0;
        static List<String> urlsList = new List<string>();

        static void Main(string[] args)
        {
            GetUrls();
            foreach (var item in urlsList)
            {
                GetResponse(item).Wait();
            }
           // GetResponse().Wait();
            Console.WriteLine(count);

        }

        static void GetUrls()
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();

            doc = hw.Load("https://api.walletinsights.com");
            foreach (HtmlNode item in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                string hrefValue = item.GetAttributeValue("href", string.Empty);
                hrefValue =  hrefValue.Replace("/help", "/ping");
                urlsList.Add(hrefValue);
                Console.WriteLine(hrefValue);
            }


        }
        static async Task GetResponse(string urlAppend)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.walletinsights.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicatoin/json"));


                HttpResponseMessage response = await client.GetAsync(urlAppend);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (String.Equals(result,"\"pong\""))
                    {
                        count++;
                    }
                }               
            }
        }
    }
}
