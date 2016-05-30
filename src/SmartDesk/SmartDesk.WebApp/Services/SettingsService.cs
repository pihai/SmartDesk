using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDesk.WebApp.Services {
  public class SettingsService : ISettingsService {

    private static Settings _settings = new Settings(new TimeSpan(4, 30, 0), "1", 80);

    public IEnumerable<string> AvailableDevices() {
      return new List<string> { "1" };
    }

    public Settings LoadSettings() {
      return _settings;
    }

    public void SaveSettings(Settings settings) {
      _settings = settings;
    }
  }
}
