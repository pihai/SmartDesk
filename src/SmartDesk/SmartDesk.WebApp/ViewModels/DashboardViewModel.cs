using System;

namespace SmartDesk.WebApp.ViewModels {
  public class DashboardViewModel {
    public DashboardViewModel(int deviceId, TimeSpan standingTarget) {
      StandingTarget = standingTarget;
      DeviceId = deviceId;
    }

    public TimeSpan StandingTarget { get; }
    public int DeviceId { get; }
    public string StandingTodayText => TimeSpanToDisplayText(StandingTarget);

    private static string TimeSpanToDisplayText(TimeSpan t) => $"{t.Hours} h {t.Minutes} min";
  }
}