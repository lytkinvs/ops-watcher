using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OpsWatcher.Web.Models;
using OpsWatcher.Web.services;

namespace OpsWatcher.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataService _dataService;

    public HomeController(ILogger<HomeController> logger, DataService dataService)
    {
        _logger = logger;
        _dataService = dataService;
    }

    public async Task<IActionResult> Index()
    {
        return View();
        return Ok(_dataService.GetItems());
    }
    
}
