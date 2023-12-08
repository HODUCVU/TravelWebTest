using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDuLichMVC.ViewModels
{
    public class FeedbackCreateViewModel
    {
        public string Name { get; set; }
        public string Location { get; set; }   
        public string Description { get; set; }
        public int Rating { get; set; }
        public string FDescription { get; set; }
        public int PackageId { get; set; }
        public string UserId { get; set; }
    }
}