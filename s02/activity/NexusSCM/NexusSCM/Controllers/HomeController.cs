using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NexusSCM.Models;

namespace NexusSCM.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["HomeMessage"] = "to the Nexus Supply Chain Management System!";
        return View();
    }

    public IActionResult Privacy()
    {
        ViewData["PrivacyMessage"] = "This policy was last updated on today's date.";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
