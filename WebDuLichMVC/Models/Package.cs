using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDuLichMVC.Models
{
    public class Package
    {
        public int PackageId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public bool isAvailable { get; set; }
        public int LocationId { get; set; }
        public string Location { get; set; } // Location Location?
        public string UserId { get; set; }
        public IEnumerable<Feedback> Feedbacks { get; set; }
    }
}