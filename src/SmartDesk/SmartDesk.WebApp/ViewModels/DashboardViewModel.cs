using System;

namespace SmartDesk.WebApp.ViewModels {
  public class DashboardViewModel {
    public DashboardViewModel(TimeSpan standingToday) {
      StandingToday = standingToday;
    }

    public TimeSpan StandingToday { get; }
    public string StandingTodayText => TimeSpanToDisplayText(StandingToday);

    private static string TimeSpanToDisplayText(TimeSpan t) => $"{t.Hours} h {t.Minutes} min";
  }
}