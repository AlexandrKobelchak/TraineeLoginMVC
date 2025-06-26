using System.ComponentModel.DataAnnotations;

namespace WebApp.Identity.ViewModels;

public class RegisterVM
{
    [Required] 
    public string? Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string?  Email { get; set; }
    
    [Required] 
    public string?  Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Password dont match.")] 
    public string? ConfirmPassword { get; set; }
}