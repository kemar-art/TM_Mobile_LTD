using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDiSpotShop.Shared.Modles
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 9)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "The Password on not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
