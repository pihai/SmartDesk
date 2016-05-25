using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmartDesk.WebApp.Queries.Dtos {
  public class DayHistoryEntry {
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [JsonConverter(typeof (StringEnumConverter))]
    public ActivityType ActivityType { get; set; }

    public DayHistoryEntry(DateTime startDate, DateTime endDate, ActivityType activityType) {
      StartDate = startDate;
      EndDate = endDate;
      ActivityType = activityType;
    }
  }
}