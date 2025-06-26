using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Identity;
using WebApp.Identity.ViewModels;

namespace WebApp.Controllers;

public class AccountController :Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    [HttpPost]
    public IActionResult Login([FromForm] LoginVM model)
    {
        
        return View();
    }
    
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register([FromForm] RegisterVM model)
    {
        
        return View();
    }
     
    public IActionResult Register()
    {
        return View();
    }
}