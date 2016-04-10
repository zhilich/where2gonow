using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Http;
using TripAdvisor;
using Where2GoNow.Utils;

namespace Where2GoNow.Controllers
{
    public class CategoriesController : ApiController
    {
        // GET: api/categories
        [HttpGet]
        public IDictionary<string, string[]> Get()
        {
            return GeoSearch.Categories;
        }
    }
}
