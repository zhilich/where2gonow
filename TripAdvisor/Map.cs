using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TripAdvisor
{
  [DataContract]
  public class Map
  {
    [DataMember(Name = "hotels")]
    public IList<Hotel> hotels { get; set; }

    [DataMember(Name = "restaurants")]
    public IList<Restaurant> restaurants { get; set; }

    [DataMember(Name = "attractions")]
    public IList<Attraction> attractions { get; set; }

    [DataMember(Name = "vacationrentals")]
    public IList<object> vacationrentals { get; set; }

    [DataMember(Name = "hotelsVisible")]
    public bool hotelsVisible { get; set; }

    [DataMember(Name = "disneyVisible")]
    public bool disneyVisible { get; set; }

    [DataMember(Name = "sponsorVisible")]
    public bool sponsorVisible { get; set; }

    [DataMember(Name = "addressInfo")]
    public AddressInfo addressInfo { get; set; }

    [DataMember(Name = "homeSponsored")]
    public bool homeSponsored { get; set; }

    [DataMember(Name = "filterState")]
    public FilterState filterState { get; set; }
  }
}
