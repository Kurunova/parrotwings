using System;
using System.ComponentModel.DataAnnotations;
using BusinessLogicLayer.ValidationAttributes;

namespace BusinessLogicLayer.Models
{
    public class User 
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+.[a-zA-Z0-9_-]+$", ErrorMessage = "Incorrect format")]
        [StringLength(20)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [IsEqual(nameof(PasswordRepeat))]
        [StringLength(20)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [StringLength(20)]
        public string PasswordRepeat { get; set; }

        [Required]
        [Display(Name = "First name")]
        [RegularExpression(@"^[a-zA-Z]+", ErrorMessage = "Incorrect format")]
        [StringLength(20)]
        public string Name { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}