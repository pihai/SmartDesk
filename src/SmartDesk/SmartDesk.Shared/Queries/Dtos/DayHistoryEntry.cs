using System;

namespace SmartDesk.Shared.Queries.Dtos {
  public class DayHistoryEntry {
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ActivityType ActivityType { get; set; }

    public DayHistoryEntry(DateTime startDate, DateTime endDate, ActivityType activityType) {
      StartDate = startDate;
      EndDate = endDate;
      ActivityType = activityType;
    }
  }
}