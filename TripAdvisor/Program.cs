using System;

namespace TripAdvisor
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      Crawler crawler = new Crawler();
      crawler.Delay = TimeSpan.FromSeconds(1.0);
      crawler.Crawl(49.310786, -126.363568, 24.056479, -68.718305, 13.0, 1000.0);
      crawler.Save("map.json");
    }
  }
}
