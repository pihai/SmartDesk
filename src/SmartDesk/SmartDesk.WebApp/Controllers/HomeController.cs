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

    public async Task<IActionResult> Index() {
      var settings = await _settingsService.LoadSettings(1);
      var viewModel = new DashboardViewModel(settings.DeviceId, settings.StandingTarget);
      return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Settings() {
      var settings = await _settingsService.LoadSettings(1);
      return View(new SettingsViewModel {
        DeviceId = settings.DeviceId,
        Height = settings.Height,
        StandingTarget = settings.StandingTarget
      });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(SettingsViewModel model) {
      await _settingsService.SaveSettings(new Settings(model.StandingTarget, model.DeviceId, model.Height));
      return RedirectToAction(nameof(Settings));
    }

    public IActionResult Error() {
      return View();
    }
  }
}
