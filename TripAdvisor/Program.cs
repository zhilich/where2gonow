using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TripAdvisor
{
    static class Program
    {
        static void Main()
        {
            var crawler = new Crawler();
            crawler.Delay = TimeSpan.FromSeconds(1);
            crawler.Crawl(49.310786, -126.363568, 24.056479, -68.718305, 13, 1000);
            crawler.Save("map.json");
        }
    }
}
