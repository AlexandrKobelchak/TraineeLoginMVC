using System.Security.Policy;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Identity;
using WebApp.Identity.ViewModels;

namespace WebApp.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppSignInManager _signInManager;
    private readonly AppUserManager _userManager;

    public AccountController(ILogger<HomeController> logger, AppSignInManager signInManager, AppUserManager userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger=logger;
    }
    
    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    } 
    
    [HttpPost()]
    public async Task<IActionResult> Login( [FromForm] LoginVM model, [FromQuery(Name = "ReturnUrl")]string? returnUrl )
    {
        ViewBag.ReturnUrl = returnUrl;
        returnUrl = returnUrl ?? Url.Content("~/");
        returnUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + Url.Content(returnUrl);
        
        if (ModelState.IsValid)
        {
            var res = await Task.Run(() =>
                _signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false));
                
            if (res.Succeeded)
            {
                return Redirect(returnUrl);
            }
        }
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterVM model)
    {
        if (ModelState.IsValid)
        {
            AppUser user = new AppUser
            {
                NicName = model.Name!,
                EmailConfirmed = true,
                UserName = model.Email,
                Email = model.Email,
                Address = model.Address,
                
            };
            var result = _userManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
                //return RedirectToAction("Login", "Account", new LoginVM{Username = model.Email, Password = model.Password});
                
                return View("Login", new LoginVM{Username = model.Email, Password = model.Password});
            }
        }
        return RedirectToAction("Index", "Home");
    }
     
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    } 
}