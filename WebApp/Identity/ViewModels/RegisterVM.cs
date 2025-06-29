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
    [DataType(DataType.Password)]
    public string?  Password { get; set; }
    
    [Required] 
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password dont match.")] 
    public string? ConfirmPassword { get; set; }
   
    [StringLength(260)]
    [DataType(DataType.MultilineText)]
    public string? Address { get; set; }    
    
}