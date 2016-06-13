using System;
using SmartDesk.Shared.Queries.Dtos;

namespace SmartDesk.WebApp.Models {
  public class DayHistoryEntryDTO {
    public DayHistoryEntryDTO(DayHistoryEntry x) {
      StartDate = x.StartDate;
      EndDate = x.EndDate;
      ActivityType = x.ActivityType.ToString();
    }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ActivityType { get; set; }
  }
}