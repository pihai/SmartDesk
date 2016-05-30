using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SmartDesk.WebApp.ViewModels;
using SmartDesk.WebApp.Services;

namespace SmartDesk.WebApp.Controllers {
  public class HomeController : Controller {
    private readonly ISettingsService _settingsService;
    public HomeController(ISettingsService settingsService) {

      _settingsService = settingsService;
    }
    public IActionResult Index() {
      Settings settings = _settingsService.LoadSettings();
      var viewModel = new DashboardViewModel(settings.DeviceId,settings.StandingTarget);
      return View(viewModel);
    }

    public IActionResult About() {
      ViewData["Message"] = "Your application description page.";

      return View();
    }

    public IActionResult Contact() {
      ViewData["Message"] = "Your contact page.";

      return View();
    }
    [HttpGet]
    public IActionResult Settings() {
      Settings settings = _settingsService.LoadSettings();
      return View(new SettingsViewModel {
        DeviceId = settings.DeviceId,
        Height = settings.Height,
        StandingTarget = settings.StandingTarget,
        Devices = _settingsService.AvailableDevices()
      });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public  IActionResult Settings(SettingsViewModel model) {

      _settingsService.SaveSettings(new Settings(model.StandingTarget, model.DeviceId, model.Height));
      return RedirectToAction(nameof(Settings));
    }
    public IActionResult Error() {
      return View();
    }
  }
}
