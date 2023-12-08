using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDuLichMVC.Models;

namespace WebDuLichMVC.ViewModels
{
    public class PackageDetailViewModel
    {
        public int PackageId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public bool IsAvailable { get; set; }
        public IEnumerable<Feedback> Feedbacks { get; set; }
    }
}