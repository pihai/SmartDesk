using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDesk.WebApp.Services {
  public interface ISettingsService {
    Settings LoadSettings();
    void SaveSettings(Settings settings);
    IEnumerable<string> AvailableDevices();
  }
  public class Settings {

    public TimeSpan StandingTarget { get; set; }

    public string DeviceId { get; set; }

    public int Height { get; set; }

    public Settings(TimeSpan standingTarget, string deviceId, int height) {
      StandingTarget = standingTarget;
      DeviceId = deviceId;
      Height = height;
    }
  }
}
