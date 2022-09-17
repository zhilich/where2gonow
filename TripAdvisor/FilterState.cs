using System.Runtime.Serialization;

namespace TripAdvisor
{
  [DataContract]
  public class FilterState
  {
    [DataMember(Name = "cat")]
    public int cat { get; set; }
  }
}
