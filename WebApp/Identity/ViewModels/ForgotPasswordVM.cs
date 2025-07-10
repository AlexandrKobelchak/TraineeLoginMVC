using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class ForgotPasswordVM
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
}