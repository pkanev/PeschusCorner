namespace Peschu.Web.Areas.Admin.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    public class AdminChangePasswordModel
    {
        public string Email { get; set; }
                
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = WebConstants.MinimumPasswordLength)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
