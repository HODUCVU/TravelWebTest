using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebDuLichMVC.Models;
using WebDuLichMVC.Services;

namespace WebDuLichMVC.Controllers.Api
{
    public class PackageApiController : Controller
    {
        private IDataService<Package> _packageDataService;
        private IDataService<Location> _locationDataServce;
        public PackageApiController(IDataService<Package> packageService,
                                    IDataService<Location> locationService)
        {
            _packageDataService = packageService;
            _locationDataServce = locationService;
        }
        [HttpGet("api/packagegetall")]
        public JsonResult GetAllPackages()
        {
            try
            {
                IEnumerable<Package> packageList = _packageDataService.GetAll();
                return Json(packageList);
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new {message = e.Message});
            }
        }

        [HttpGet("api/packagegetbylocation")]
        public JsonResult GetPackageByLocation(string locationName)
        {
            try
            {
                IEnumerable<Package> packages = _packageDataService.Query(p =>
                p.Location.ToLower() == locationName.ToLower());
                if(packages != null)
                {
                    return Json(packages);
                }
                return Json(new {message = "Cannot find any packages"});
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new {message = e.Message});
            }
        }
    }
}