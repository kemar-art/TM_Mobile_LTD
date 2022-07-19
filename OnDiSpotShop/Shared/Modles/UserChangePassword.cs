using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDiSpotShop.Shared.Modles
{
    public class UserChangePassword
    {
        [Required, StringLength(100, MinimumLength = 9)]
        public string Password { get; set; } = String.Empty;
        [Compare("Password", ErrorMessage = "The Password on not match")]
        public string ConfirmPassword { get; set; } = String.Empty;

    }
}
