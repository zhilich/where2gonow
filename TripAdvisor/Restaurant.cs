using System.Runtime.Serialization;

namespace TripAdvisor
{
  [DataContract]
  public class Restaurant
  {
    [DataMember(Name = "lat")]
    public double lat { get; set; }

    [DataMember(Name = "lng")]
    public double lng { get; set; }

    [DataMember(Name = "locId")]
    public int locId { get; set; }

    [DataMember(Name = "url")]
    public string url { get; set; }

    [DataMember(Name = "overviewWeight")]
    public double overviewWeight { get; set; }

    [DataMember(Name = "accommodationCategory")]
    public int accommodationCategory { get; set; }

    [DataMember(Name = "customHover")]
    public CustomHover customHover { get; set; }

    [DataMember(Name = "pinProminent")]
    public bool pinProminent { get; set; }
  }
}
