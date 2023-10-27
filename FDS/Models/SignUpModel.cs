using System.ComponentModel.DataAnnotations;

namespace FDS.Models
{
    public class SignUpModel
    {
        public string Name { get; set; } = null!;
        public DateTime? DateOfBirt { get; set; }
        public string Gender { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;
    }
}
