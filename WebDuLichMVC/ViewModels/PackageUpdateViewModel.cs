using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebDuLichMVC.ViewModels
{
    public class PackageUpdateViewModel
    {
        public int PackageId { get; set; }
        [Required]
        [MaxLength(100)]
        public string PackageName { get; set; }
        public string LocationName { get; set; }
        public SelectList LocationList { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }
        public string Picture { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        public int LOcationId { get; set; }
        public string UserId { get; set; }
    }
}