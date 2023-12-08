using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebDuLichMVC.Models;
using WebDuLichMVC.Services;
using WebDuLichMVC.ViewModels;

namespace WebDuLichMVC.Controllers
{
    public class LocationController : Controller
    {
        private IDataService<Location> _locationDataService;
        private IDataService<Package> _packageDataService;
        public LocationController(IDataService<Package> packageService,
                                IDataService<Location> locationService)
        {
            _packageDataService = packageService;
            _locationDataService = locationService;
        }

        [HttpGet]
        public IActionResult Details(int id, string name)
        {
            TempData["locationId"] = id.ToString();

            Location location = _locationDataService.GetSignle(p => p.LocationId == id);
            IEnumerable<Package> packages = _packageDataService.Query(p => p.LocationId == id
                                                                    && p.isAvailable == true);
            
            LocationDetailsViewModel vm = new LocationDetailsViewModel {
                Name = location.Name,
                Description = location.Description,
                Picture = location.Picture,
                Packages = packages
            };
            return View(vm);
        }
    }
}