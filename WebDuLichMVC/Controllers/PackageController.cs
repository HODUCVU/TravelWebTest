using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Wasm;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Crypto.Engines;
using WebDuLichMVC.Models;
using WebDuLichMVC.Services;
using WebDuLichMVC.ViewModels;

namespace WebDuLichMVC.Controllers
{
    public class PackageController : Controller
    {
        private UserManager<IdentityUser> _userManagerService;
        private IDataService<Location> _locationDataService;
        private IDataService<Package> _packageDataService;
        private IDataService<Feedback> _feedbackDataService;
        private IWebHostEnvironment _environment;
        public PackageController(UserManager<IdentityUser> userManager,
                                IDataService<Package> packageService,
                                IDataService<Location> locationService,
                                IDataService<Feedback> feedbackService,
                                IWebHostEnvironment environment)
        {
            _userManagerService = userManager;
            _packageDataService = packageService;
            _locationDataService = locationService;
            _feedbackDataService = feedbackService;
            _environment = environment;
        }

        [HttpGet]
        [Authorize(Roles = "Provider")]
        public IActionResult Create(int id)
        {
            Location location = _locationDataService.GetSignle(p => p.LocationId == id);
            PackageCreateViewModel vm = new PackageCreateViewModel {
                LOcationId = location.LocationId,
                LocationName = location.Name
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Create(PackageCreateViewModel vm, IFormFile file)
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            if(ModelState.IsValid)
            {
                Package existingPackage = _packageDataService.GetSignle(p => p.Name == vm.PackageName);
                if(existingPackage == null)
                {
                    Package package = new Package {
                        Name = vm.PackageName,
                        Price = vm.Price,
                        Location = vm.LocationName,
                        Description = vm.Description,
                        isAvailable = true,
                        LocationId = vm.LOcationId,
                        UserId = user.Id
                    };
                    if(file != null)
                    {
                        var serverPath = Path.Combine(_environment.WebRootPath, "uploads/package");
                        Directory.CreateDirectory(Path.Combine(serverPath,User.Identity.Name));
                        string filename = FileNameHelper.GetNameFormatted(Path.GetFileName(file.FileName));
                        using(var fileStream = new FileStream(Path.Combine(serverPath, User.Identity.Name, filename), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        package.Picture = User.Identity.Name + "/" +filename;
                    }
                    else if(file == null)
                    {
                        package.Picture = "apackagegen.jpg";
                    }
                    _packageDataService.Create(package);
                    return RedirectToAction("Details", "Package", new {id = package.PackageId});
                }
                else 
                {
                    ViewBag.MyMessage =  "Package name exists. please change the name";
                    return View(vm);
                }
            }
            return View(vm);
        }

        [HttpGet]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Update(int id)
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            Package package = _packageDataService.GetSignle(p => p.PackageId == id);
            IEnumerable<Location> locations = _locationDataService.GetAll();
            if(package.UserId == user.Id)
            {
                PackageUpdateViewModel vm = new PackageUpdateViewModel{
                    PackageId = package.PackageId,
                    PackageName = package.Name,
                    LocationName = package.Location,
                    Price = package.Price,
                    Description = package.Description,
                    Picture = package.Picture,
                    IsAvailable = package.isAvailable,
                    LOcationId = package.LocationId,
                    UserId = package.UserId
                    //LocationList = ViewBag.LocationName = new SelectList(locationList, "LocationId", "Name"),
                };
                return View(vm);
            }
            return RedirectToAction("Denied", "Account");
            // or home
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> Update(PackageUpdateViewModel vm, IFormFile file)
        {
            if(ModelState.IsValid)
            {
                Package package = _packageDataService.GetSignle(p => p.PackageId == vm.PackageId);
                package.PackageId = vm.PackageId;
                package.Name = vm.PackageName;
                package.Location = vm.LocationName;
                package.Price = vm.Price;
                package.Description = vm.Description;
                package.isAvailable = vm.IsAvailable;
                package.LocationId = vm.LOcationId;
                package.UserId = vm.UserId;
                if(file != null)
                {
                    var serverPath = Path.Combine(_environment.WebRootPath, "uploads/package");
                    Directory.CreateDirectory(Path.Combine(serverPath,User.Identity.Name));
                    string filename = FileNameHelper.GetNameFormatted(Path.GetFileName(file.FileName));
                    using(var fileStream = new FileStream(Path.Combine(serverPath,User.Identity.Name, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    package.Picture = User.Identity.Name + "/" + filename;
                }
                _packageDataService.Update(package);
                return RedirectToAction("Details", "Package", new{id = package.PackageId});
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            TempData["packageId"] = id.ToString();
            Package package = _packageDataService.GetSignle(p => p.PackageId == id);
            IEnumerable<Feedback> feedbacks = _feedbackDataService.Query(f =>f.PackageId ==id);

            PackageDetailViewModel vm = new PackageDetailViewModel {
                Name = package.Name,
                Location = package.Location,
                Price = package.Price,
                Description = package.Description,
                Picture = package.Picture,
                IsAvailable = package.isAvailable,
                Feedbacks = feedbacks
            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult Search(PackageSearchViewModel vm)
        {
            IEnumerable<Package> packages;
            if(vm != null)
            {
                if(!string.IsNullOrEmpty(vm.SearchString))
                {
                    packages = _packageDataService.Query(p =>
                        p.Location.Contains(vm.SearchString) && p.isAvailable == true);
                        if(vm.MinPrice.HasValue && vm.MinPrice.HasValue)
                        {
                            packages = packages.Where(p => p.Price <= vm.MaxPrice && p.Price >= vm.MinPrice);
                        } 
                        else if(!vm.MinPrice.HasValue && vm.MaxPrice.HasValue)
                        {
                            packages = packages.Where(p => p.Price <= vm.MaxPrice);
                        }
                        else if(vm.MinPrice.HasValue && !vm.MaxPrice.HasValue)
                        {
                            packages = packages.Where(p => p.Price >= vm.MinPrice);
                        }
                        vm.Packages = packages;
                        vm.Total = packages.Count();
                }
                else if(string.IsNullOrEmpty(vm.SearchString) && (vm.MinPrice.HasValue || vm.MaxPrice.HasValue))
                {
                    if(vm.MinPrice.HasValue && vm.MaxPrice.HasValue)
                    {
                        packages = _packageDataService.Query(p => p.Price <= vm.MaxPrice && 
                                                        p.Price >= vm.MinPrice && p.isAvailable == true);
                    }
                    else if(!vm.MinPrice.HasValue && vm.MaxPrice.HasValue)
                    {
                        packages = _packageDataService.Query(p => p.Price <= vm.MaxPrice && p.isAvailable == true);
                    }
                    else
                    {
                        packages = _packageDataService.Query(p => p.Price >= vm.MinPrice && p.isAvailable == true);
                    }
                    vm.Packages = packages;
                    vm.Total = packages.Count();
                } 
                else 
                {
                    ViewBag.PackageSearch =  "No packages available. Please try another location or price";
                }
                return View(vm);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Provider")]
        public IActionResult CreateSelect()
        {
            IEnumerable<Location> locations = _locationDataService.GetAll();

            PackageCreateSelectViewModel vm = new PackageCreateSelectViewModel {
                Locations = locations
            };
            return View(vm);
        }
        [HttpGet]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> UpdateSelect()
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            IEnumerable<Package> packages = _packageDataService.Query(p => p.UserId == user.Id);

            PackageUpdateSelectViewModel vm = new PackageUpdateSelectViewModel {
                Packages = packages,
                Count = packages.Count()
            };
            return View(vm);
        }
    }
}