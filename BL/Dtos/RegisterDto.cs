using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dtos
{
    public class RegisterDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string PasswordHash { get; set; }

        [Compare("PasswordHash")]
        public string ConfirmPassword { get; set; }        
        public string RoleName { get; set; }


    }
}
