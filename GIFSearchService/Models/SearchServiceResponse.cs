using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GIFSearchService.Common.Response
{   
    public class SearchServiceResponse
    {
        public List<SearchServiceResponseItem> data { get; set; }
    }

    public class SearchServiceResponseItem
    {
        public string gif_id { get; set; }
        public string url { get; set; }
    }
}