using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Threading;

namespace TripAdvisor
{
  public class Crawler
  {
    private Regex imgRegex = new Regex("https://media-cdn.tripadvisor.com/media/photo-[^']+.jpg");
    private Regex reviewsRegex = new Regex("(?<reviews>[0-9,.]+) review");
    private Regex ratingRegex = new Regex("src=\"https://static.tacdn.com/img2/x.gif\" alt=\"(?<rating>[0-9,.]+) of 5 stars\"");
    private Regex categoryRegex = new Regex("<div class=\"gray-footer \">(?<category>.*?)<\\/div>", RegexOptions.Singleline);
    private HashSet<Attraction> _attractions = new HashSet<Attraction>();
    private StreamWriter _log = new StreamWriter("log.txt", false);

    public TimeSpan Delay { get; set; }

    public IEnumerable<Attraction> Attractions => (IEnumerable<Attraction>) this._attractions;

    public void Crawl(
      double startLat,
      double startLng,
      double endLat,
      double endLng,
      double zoom,
      double size)
    {
      double num1 = (startLat - endLat) * (endLng - startLng);
      for (double lat = endLat; lat <= startLat; lat += zoom * 10.0 / size)
      {
        for (double lng = startLng; lng <= endLng; lng += zoom * 10.0 / size)
        {
          double num2 = (lat - endLat) * (endLng - startLng) + (lng - startLng) * zoom * 10.0 / size;
          this.ReadSegment(lat, lng, zoom, size);
          this.Log(string.Format("{0}% complete", (object) (num2 / num1 * 100.0)));
        }
      }
    }

    public void Save(string filename)
    {
      using (FileStream fileStream = new FileStream(filename, (FileMode) 2))
        new DataContractJsonSerializer(typeof (List<Attraction>), new DataContractJsonSerializerSettings()
        {
          UseSimpleDictionaryFormat = true
        }).WriteObject((Stream) fileStream, (object) this._attractions);
    }

    public void Log(string message)
    {
      Console.WriteLine(message);
      ((TextWriter) this._log).WriteLine(message);
      ((TextWriter) this._log).Flush();
    }

    private void ReadSegment(double lat, double lng, double zoom, double size)
    {
      WebClient webClient = new WebClient();
      Directory.CreateDirectory("download");
      string path1 = string.Format("download\\{0}_{1}_{2}_{3}.json", (object) lat.ToString("0.0000"), (object) lng.ToString("0.0000"), (object) zoom, (object) size);
      byte[] bytes;
      if (!File.Exists(path1))
      {
        string str = string.Format("https://www.tripadvisor.com/GMapsLocationController?Action=update&from=Attractions&g=35805&geo=35805&mapProviderFeature=ta-maps-gmaps3&validDates=false&mc={0},{1}&mz={2}&mw={3}&mh={4}&pinSel=v2&origLocId=35805&sponsors=&finalRequest=false&includeMeta=false&trackPageView=false", (object) lat, (object) lng, (object) zoom, (object) size, (object) size);
        Thread.Sleep(TimeSpan.FromSeconds(0.1));
        bytes = webClient.DownloadData(str);
        this.Log(string.Format("Read segment {0}, {1}. {2} bytes.", (object) lat, (object) lng, (object) bytes.Length));
        if (bytes.Length > 400000)
          throw new Exception("Too much items. Increase zoom.");
        File.WriteAllBytes(path1, bytes);
      }
      else
        bytes = File.ReadAllBytes(path1);
      using (MemoryStream memoryStream = new MemoryStream(bytes))
      {
        Map map = (Map) new DataContractJsonSerializer(typeof (Map)).ReadObject((Stream) memoryStream);
        this.Log(string.Format("Found {0} attractions", (object) map.attractions.Count));
        foreach (Attraction attraction in (IEnumerable<Attraction>) map.attractions)
        {
          this.Log("Attraction: " + attraction.customHover.title);
          string path2 = string.Format("download\\{0}.html", (object) attraction.locId);
          string str1;
          if (!File.Exists(path2))
          {
            string str2 = "https://www.tripadvisor.com" + attraction.customHover.url.Replace("Action=info", "Action=infoCardAttr");
            this.Log(string.Format("{0}: {1}", (object) attraction.locId, (object) str2));
            Thread.Sleep(this.Delay);
            str1 = webClient.DownloadString(str2);
            File.WriteAllText(path2, str1);
          }
          else
            str1 = File.ReadAllText(path2);
          string str3 = this.imgRegex.Match(str1).Value;
          string s1 = this.reviewsRegex.Match(str1).Groups["reviews"].Value;
          string s2 = this.ratingRegex.Match(str1).Groups["rating"].Value;
          string str4 = this.categoryRegex.Match(str1).Groups["category"].Value.Trim('\r', '\n', ' ');
          this.Log("reviews: " + s1 + ", rating: " + s2 + ", categories: " + str4 + ", image: " + str3);
          attraction.categories = ((IEnumerable<string>) (str4 ?? string.Empty).Split(new char[1]
          {
            ','
          }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((Func<string, string>) (_c => _c.Trim(' '))).ToList<string>();
          attraction.imgUrl = str3;
          if (!string.IsNullOrEmpty(s1))
            attraction.reviews = int.Parse(s1, NumberStyles.AllowThousands);
          if (!string.IsNullOrEmpty(s2))
            attraction.rating = float.Parse(s2);
          if (!this._attractions.Contains(attraction))
            this._attractions.Add(attraction);
          else
            this.Log("Attraction " + attraction.customHover.title + " already exists.");
        }
      }
    }
  }
}
