// Decompiled with JetBrains decompiler
// Type: Where2GoNow.Controllers.AttractionsController
// Assembly: Where2GoNow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41A384F9-EA7D-429A-9668-A0FAC07AF389
// Assembly location: C:\Users\Raman\Downloads\Prod-AspNetCoreFunction-11ROF6BHOWIHX-d59f0e52-bd55-4f63-9121-40f7e7fc781b\Where2GoNow.dll

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TripAdvisor;
using Where2GoNow.DataAccess.Repositories;
using Where2GoNow.Utils;

namespace Where2GoNow.Controllers
{
  [Route("api/[controller]")]
  public class AttractionsController : Controller
  {
    private AttractionRepository _attractionRepository;

    public AttractionsController(AttractionRepository attractionRepository) => this._attractionRepository = attractionRepository;

    [HttpGet]
    public async Task<IActionResult> Get(
      double lat,
      double lng,
      double radius = 10.0,
      double popularity = 0.0,
      string categories = "")
    {
      AttractionsController attractionsController = this;
      try
      {
        IEnumerable<Attraction> attractions = await attractionsController._attractionRepository.GetAttractions(lat, lng, radius, Convert.ToInt32(popularity));
        List<Attraction> attractionList = new List<Attraction>();
        foreach (Attraction attraction in attractions)
        {
          if (GeoCodeCalc.CalcDistance(lat, lng, attraction.lat, attraction.lng) <= radius && (double) attraction.reviews >= popularity && GeoSearch.HasCategory(attraction, categories))
            attractionList.Add(attraction);
        }
        return (IActionResult) ((ControllerBase) attractionsController).Ok((object) attractionList);
      }
      catch (Exception ex)
      {
        return (IActionResult) ((ControllerBase) attractionsController).StatusCode(500, (object) ex.ToString());
      }
    }
  }
}
