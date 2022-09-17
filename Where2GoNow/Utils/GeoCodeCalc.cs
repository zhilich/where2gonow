// Decompiled with JetBrains decompiler
// Type: Where2GoNow.Utils.GeoCodeCalc
// Assembly: Where2GoNow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41A384F9-EA7D-429A-9668-A0FAC07AF389
// Assembly location: C:\Users\Raman\Downloads\Prod-AspNetCoreFunction-11ROF6BHOWIHX-d59f0e52-bd55-4f63-9121-40f7e7fc781b\Where2GoNow.dll

using System;

namespace Where2GoNow.Utils
{
  public static class GeoCodeCalc
  {
    public const double EarthRadiusInMiles = 3956.0;
    public const double EarthRadiusInKilometers = 6367.0;

    public static double ToRadian(double val) => val * (Math.PI / 180.0);

    public static double DiffRadian(double val1, double val2) => GeoCodeCalc.ToRadian(val2) - GeoCodeCalc.ToRadian(val1);

    public static double CalcDistance(double lat1, double lng1, double lat2, double lng2) => GeoCodeCalc.CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Miles);

    public static double CalcDistance(
      double lat1,
      double lng1,
      double lat2,
      double lng2,
      GeoCodeCalcMeasurement m)
    {
      double num = 3956.0;
      if (m == GeoCodeCalcMeasurement.Kilometers)
        num = 6367.0;
      return num * 2.0 * Math.Asin(Math.Min(1.0, Math.Sqrt(Math.Pow(Math.Sin(GeoCodeCalc.DiffRadian(lat1, lat2) / 2.0), 2.0) + Math.Cos(GeoCodeCalc.ToRadian(lat1)) * Math.Cos(GeoCodeCalc.ToRadian(lat2)) * Math.Pow(Math.Sin(GeoCodeCalc.DiffRadian(lng1, lng2) / 2.0), 2.0))));
    }
  }
}
