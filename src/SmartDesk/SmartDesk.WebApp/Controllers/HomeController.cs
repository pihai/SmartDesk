using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SmartDesk.WebApp.ViewModels;

namespace SmartDesk.WebApp.Controllers {
  public class HomeController : Controller {
    public IActionResult Index() {
      var viewModel = new DashboardViewModel(TimeSpan.FromHours(2.34));
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

    public IActionResult Error() {
      return View();
    }
  }
}
