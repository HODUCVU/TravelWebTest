using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebDuLichMVC.ViewModels
{
    public class AccountUpdateProvProfileViewModel
    {
        public string CompanyName { get; set; } 
        [DataType(DataType.Url, ErrorMessage ="Website must be valid Url!")]
        public string Website { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage ="Phone number is wrong!")]
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CompanyLogo { get; set; }
    }
}