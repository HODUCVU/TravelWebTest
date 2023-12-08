using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDuLichMVC.Models;

namespace WebDuLichMVC.ViewModels
{
    public class PackageSearchViewModel
    {
        public int Total { get; set; } 
        public string SearchString { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public IEnumerable<Package> Packages { get; set; }
    }
}