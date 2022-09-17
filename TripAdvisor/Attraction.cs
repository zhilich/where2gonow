using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TripAdvisor
{
  [DataContract]
  public class Attraction
  {
    [DataMember(Name = "lat")]
    public double lat { get; set; }

    [DataMember(Name = "lng")]
    public double lng { get; set; }

    [DataMember(Name = "locId")]
    public int locId { get; set; }

    [DataMember(Name = "url")]
    public string url { get; set; }

    [DataMember(Name = "customHover")]
    public CustomHover customHover { get; set; }

    [DataMember(Name = "imgUrl")]
    public string imgUrl { get; set; }

    [DataMember(Name = "rating")]
    public float rating { get; set; }

    [DataMember(Name = "reviews")]
    public int reviews { get; set; }

    [DataMember(Name = "categories")]
    public List<string> categories { get; set; }

    public override bool Equals(object obj) => obj is Attraction attraction && attraction.url == this.url;

    public override int GetHashCode() => this.url.GetHashCode();
  }
}
