using System.ComponentModel.DataAnnotations;

namespace Task2.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("NewPassword", ErrorMessage = "Confirm Password does not Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
