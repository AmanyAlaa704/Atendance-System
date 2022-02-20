using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dtos
{
    public class LoginDto
    {
        public string Id { get; set; }

        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string PasswordHash { get; set; }
    }
}
