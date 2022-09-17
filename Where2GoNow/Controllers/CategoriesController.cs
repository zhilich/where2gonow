// Decompiled with JetBrains decompiler
// Type: Where2GoNow.Controllers.CategoriesController
// Assembly: Where2GoNow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41A384F9-EA7D-429A-9668-A0FAC07AF389
// Assembly location: C:\Users\Raman\Downloads\Prod-AspNetCoreFunction-11ROF6BHOWIHX-d59f0e52-bd55-4f63-9121-40f7e7fc781b\Where2GoNow.dll

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Where2GoNow.Utils;

namespace Where2GoNow.Controllers
{
  [Route("api/[controller]")]
  public class CategoriesController : Controller
  {
    [HttpGet]
    public IDictionary<string, string[]> Get() => GeoSearch.Categories;
  }
}
