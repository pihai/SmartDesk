using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartDesk.WebApp.ViewModels {
  public class SettingsViewModel {
    [Required]
    public TimeSpan StandingTarget { get; set; }
    [Required]
    public string DeviceId { get; set; }
    [Required]
    public int Height { get; set; }
    public IEnumerable<string> Devices { get; set; }
    //public IEnumerable<string> Devices
    //{
    //  get {
    //    return clubs;
    //  }
    //  set {
    //    clubs = value;

    //    if (clubs == null || clubs.Count() == 0) {
    //      ClubId = 0;
    //    }
    //    else {
    //      ClubId = clubs.First().Id;
    //    }
    //  }
    //}
  }
}