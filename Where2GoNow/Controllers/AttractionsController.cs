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
    public class AttractionsController : ApiController
    {
        private static IEnumerable<Attraction> _attractions;

        static AttractionsController()
        {
            using (FileStream stream = new FileStream(HttpContext.Current.Server.MapPath("~/App_Data/map.json") , FileMode.Open))
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(IEnumerable<Attraction>));
                _attractions = (IEnumerable<Attraction>)jsonSerializer.ReadObject(stream);
            }

        }

        // GET: api/Attractions
        [HttpGet]
        public IEnumerable<Attraction> Get(double lat, double lng, double radius = 10, double popularity = 0, string categories = "")
        {
            foreach (var attraction in _attractions)
            {
                if (GeoCodeCalc.CalcDistance(lat, lng, attraction.lat, attraction.lng) > radius) continue;
                if (attraction.reviews < popularity) continue;
                if (!GeoSearch.HasCategory(attraction, categories)) continue;
                yield return attraction;
            }
        }
    }
}
