using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft;
using System.Configuration;
using GIFSearchService.Common.Response;
using GIFSearchService.Interfaces;



namespace GIFSearchService.ExternalService
{
/// <summary>
/// Handled GIF external service methods 
/// </summary>
    public class GIFService: IGIFService
    {
        //Getting global , environment speicfic values from config file
        private readonly string gifHostUrl = ConfigurationManager.AppSettings["GifHostUrl"];
        private readonly string apiKey = ConfigurationManager.AppSettings["api_key"];
        private readonly string limit = ConfigurationManager.AppSettings["limit"];

       /// <summary>
       /// Making external service call and building the response
       /// </summary>
       /// <param name="searchTerm"></param>
       /// <returns></returns>
        
        public SearchServiceResponse GetExternalResponse(string searchTerm)
        {
            var client = new HttpClient();
            string url = gifHostUrl + "api_key=" + apiKey + "&q=" + searchTerm + "&limit=" + limit;          
            var lstSearchResponse = new List<GIFSearchService.Common.Response.SearchServiceResponseItem>();

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {               
                var result = response.Content.ReadAsStringAsync().Result;
                var result_data = JsonSerializer.Deserialize<GIFResponse>(result);

                List<GIFSearchService.Common.Response.Datum> obj1 = null;
                SearchServiceResponseItem _responseItem = new SearchServiceResponseItem();

                //Handling the requirment- Return exactly 5 if there 5 or more results available.

                if (result_data.data.Count >= 5)
                {
                    obj1 = result_data.data.Take(5).ToList();

                    foreach (var obj in obj1)
                    {
                        var url1 = obj.url;
                        var gif_id = obj.id;
                        var item = new SearchServiceResponseItem();
                        item.url = url1;
                        item.gif_id = gif_id;
                        lstSearchResponse.Add(item);
                    }

                }
                else
                {
                    //Handling the requirment- Return 0 results if there are less than 5 results available.
                    var item = new SearchServiceResponseItem();
                    item.url = string.Empty;
                    item.gif_id = string.Empty;
                    lstSearchResponse.Add(item);
                }
            }
            else
            {
                Console.WriteLine("GetExternalResponse returned exception" + response.StatusCode);

                var item = new SearchServiceResponseItem();
                item.url = string.Empty;
                item.gif_id = string.Empty;
                lstSearchResponse.Add(item);
            }

            var finalresponse = new GIFSearchService.Common.Response.SearchServiceResponse
            {
                data = lstSearchResponse
            };


            return finalresponse;
        }
    }
}