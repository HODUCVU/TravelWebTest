using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using WebDuLichMVC.Models;

namespace WebDuLichMVC.ViewModels
{
    public class LocationDetailsViewModel
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public IEnumerable<Package> Packages { get; set; }
    }
}