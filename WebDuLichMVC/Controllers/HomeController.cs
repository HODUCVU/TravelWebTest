using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebDuLichMVC.Models;
using WebDuLichMVC.Services;
using WebDuLichMVC.ViewModels;

namespace WebDuLichMVC.Controllers;

public class HomeController : Controller
{
    private IDataService<Location> _locationDataService;
    private IDataService<Package> _packageDataService;
    public HomeController(IDataService<Location> locationService,
                        IDataService<Package> packageService)
    {
        _locationDataService = locationService;
        _packageDataService = packageService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        IEnumerable<Location> locations = _locationDataService.GetAll();
        IEnumerable<Package> packages = _packageDataService.GetAll().Where(
            p => p.isAvailable == true);
        HomeIndexViewModel vm = new HomeIndexViewModel {
            Locations = locations,
            Packages = packages
        };
        return View(vm);
    }

    [HttpGet]
    public IActionResult About()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Help()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Terms()
    {
        return View();
    }
}
