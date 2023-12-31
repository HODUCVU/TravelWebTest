using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebDuLichMVC.ViewModels
{
    public class AccountRegisterViewModel
    {
        [Required]
        [MaxLength(256)]
        [DataType(DataType.EmailAddress, ErrorMessage ="Enter your Email")]
        public string UserName { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public bool IsProvider { get; set; }
    }
}