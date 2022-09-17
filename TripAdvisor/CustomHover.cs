using System.Runtime.Serialization;

namespace TripAdvisor
{
  [DataContract]
  public class CustomHover
  {
    [DataMember(Name = "title")]
    public string title { get; set; }

    [DataMember(Name = "url")]
    public string url { get; set; }
  }
}
