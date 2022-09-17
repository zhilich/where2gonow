using System.Runtime.Serialization;

namespace TripAdvisor
{
  [DataContract]
  public class AddressInfo
  {
    [DataMember(Name = "name")]
    public string name { get; set; }

    [DataMember(Name = "address")]
    public string address { get; set; }
  }
}
