using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDuLichMVC.Models;

namespace WebDuLichMVC.ViewModels
{
    public class PackageCreateSelectViewModel
    {
        public IEnumerable<Location> Locations { get; set; }
    }
}