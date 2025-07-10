using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using WebApp.Identity;
using WebApp.Identity.ViewModels;
using WebApp.Models;

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
    public IActionResult Register(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterVM model, [FromQuery(Name = "ReturnUrl")]string? returnUrl )
    {
        ViewBag.ReturnUrl = returnUrl;
        returnUrl = returnUrl ?? "~/";
        returnUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + Url.Content(returnUrl);
        
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
            var result = _userManager.CreateAsync(user, model.Password!).Result;
            if (result.Succeeded)
            {
                // return View("Login", new LoginVM{Username = model.Email, Password = model.Password});
                
                var res = await Task.Run(() =>
                    _signInManager.PasswordSignInAsync(model.Email!, model.Password!, false, false));

                if (res.Succeeded)
                {
                    return Redirect(returnUrl);
                }
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

        [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordVM vm)
    {
        if (!ModelState.IsValid) return View("ForgotPassword", vm);

        var user = await _userManager.FindByEmailAsync(vm.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            ModelState.AddModelError("Email", "Invalid email address or email no confirmed");
            return View("ForgotPassword", vm);
        }

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        Console.WriteLine(code);

        return RedirectToAction("ForgotPasswordConfirmation");
    }

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }


    [HttpGet]
    public IActionResult ResetPassword(
        [FromQuery] string? code)
    {
        if (code == null)
            return BadRequest("Код сброса или email отсутствует.");

        var model = new ResetPasswordVM
        {
            Code = code
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(
        [FromForm] ResetPasswordVM model)
    {
        if (!ModelState.IsValid) return View("ResetPassword", model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("Email", "Account with this email address not found");
            return View("ResetPassword", model);
        }
        string decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
        var result = await _userManager.ResetPasswordAsync(user, decodedCode, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("ResetPasswordConfirmation");
        }
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View();
    }
    
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}