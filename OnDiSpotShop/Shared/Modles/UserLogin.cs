using System.ComponentModel.DataAnnotations;

namespace OnDiSpotShop.Shared.Modles
{
    public class UserLogin
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
