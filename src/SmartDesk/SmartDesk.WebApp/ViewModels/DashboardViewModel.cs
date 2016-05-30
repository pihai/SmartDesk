using System;

namespace SmartDesk.WebApp.ViewModels {
  public class DashboardViewModel {
    public DashboardViewModel(string deviceId, TimeSpan standingTarget) {
      StandingTarget = standingTarget;
      DeviceId = deviceId;
    }

    public TimeSpan StandingTarget { get; }
    public string DeviceId { get; }
    public string StandingTodayText => TimeSpanToDisplayText(StandingTarget);

    private static string TimeSpanToDisplayText(TimeSpan t) => $"{t.Hours} h {t.Minutes} min";
  }
}