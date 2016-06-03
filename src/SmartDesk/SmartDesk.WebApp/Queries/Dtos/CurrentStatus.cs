using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmartDesk.WebApp.Queries.Dtos {
  public class CurrentStatus {

    public bool Active { get; set; }
    public bool Online { get; set; }
    public double Height { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public ActivityType ActivityType { get; set; }

    public CurrentStatus(bool active, bool online, ActivityType activityType, double height) {
      Active = active;
      Online = online;
      ActivityType = activityType;
      Height = height;
    }
  }
}