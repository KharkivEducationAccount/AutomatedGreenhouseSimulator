using GreenhouseScada.Web.Models;
using GreenhouseScada.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GreenhouseScada.Web.Controllers
{
    [Authorize(Policy = "CanConfigure")]
    public class SettingsController : Controller
    {
        private readonly SchemeConfigService _scheme;

        public SettingsController(SchemeConfigService scheme)
        {
            _scheme = scheme;
        }

        public IActionResult Index()
        {
            return View(_scheme.GetSensors());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateMax(string topic, int max)
        {
            if (max < -50 || max > 150)
            {
                TempData["Message"] = "Threshold must be between -50 and 150.";
                return RedirectToAction("Index");
            }

            _scheme.UpdateMax(topic, max);
            TempData["Message"] = "Threshold saved by " + User.Identity?.Name;
            return RedirectToAction("Index");
        }
    }
}
