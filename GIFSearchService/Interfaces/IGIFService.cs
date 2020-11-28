using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIFSearchService.Common.Response;

namespace GIFSearchService.Interfaces
{
    interface IGIFService
    {
        SearchServiceResponse GetExternalResponse(string searchTerm);
    }
}
