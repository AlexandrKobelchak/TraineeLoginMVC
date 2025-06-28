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
    
    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    } 
    
    [HttpPost()]
    //[Route("")]
    
    
    public async Task<IActionResult> Login( [FromForm] LoginVM model, [FromQuery(Name = "ReturnUrl")]string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        returnUrl = returnUrl ?? Url.Content("~/");
        if (ModelState.IsValid)
        {
            
            
            var res = await Task.Run(() =>
                _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false));
            
            if (res.Succeeded)
            {
               
                return RedirectToAction(returnUrl);
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
                
                EmailConfirmed = true,
                UserName = model.Email,
                Email = model.Email
            };
            var result = _userManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account", new LoginVM{Username = model.Email, Password = model.Password});
            }
        }

        return RedirectToAction("Index", "Home");
    }
     

}