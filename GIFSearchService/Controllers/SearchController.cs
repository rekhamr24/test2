using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GIFSearchService.ExternalService;
using System.Text;
using GIFSearchService.Common.Response;

namespace GIFSearchService.Controllers
{
    public class SearchController : ApiController
    {
        SearchServiceResponse serviceResponse = new SearchServiceResponse();
        GIFService _service = new GIFService();
        // [System.Web.Mvc.Route("api/Search/{id:string}")]
        [System.Web.Mvc.Route("Search/{id:string}")]
        public async Task<HttpResponseMessage> Get(string id)
        {
            try
            {
                string searchTerm = id;
                //call the gif external service
                serviceResponse = _service.GetExternalResponse(searchTerm);
                
                //response data count > 1, the transaction is successful
                if (serviceResponse.data.Count > 1)                {
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    var responseString = Newtonsoft.Json.JsonConvert.SerializeObject(serviceResponse);
                    response.Content = new StringContent(responseString, Encoding.UTF8, "application/json");
                    return response;
                }
                else
                {
                    //exception occured during the gif external service call and process
                    var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    var responseString = Newtonsoft.Json.JsonConvert.SerializeObject(serviceResponse);
                    response.Content = new StringContent(responseString, Encoding.UTF8, "application/json");
                    return response;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("exceptoin from Get method"+ ex.Message);
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                var responseString = Newtonsoft.Json.JsonConvert.SerializeObject(serviceResponse);
                response.Content = new StringContent(responseString, Encoding.UTF8, "application/json");
                return response;

            }

        }

    }
}
