using System.ComponentModel.DataAnnotations;

namespace FBLARoverAgenda_Backend.Web.Areas.Identity.Models.AccountViewModels;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}