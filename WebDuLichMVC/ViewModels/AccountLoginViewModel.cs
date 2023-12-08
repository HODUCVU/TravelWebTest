using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebDuLichMVC.ViewModels
{
    public class AccountLoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage ="Enter Your Email!")]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}