using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Models
{
    public class SignInModel
    {
        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+.[a-zA-Z0-9_-]+$", ErrorMessage = "Incorrect format")]
        [StringLength(20)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(20)]
        public string Password { get; set; }
    }
}