using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using money_manager.Entities;
using money_manager.Models;
using money_manager.Services;
using System.Text;

namespace money_manager.Controllers;

public class UsersController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IEmailService emailService) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly IEmailService _emailService = emailService;

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(CreateUserViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        UserEntity usuario = new()
        {
            UserAccountName = viewModel.UserAccountName,
            Email = viewModel.Email,
        };

        IdentityResult result = await _userManager.CreateAsync(usuario, password: viewModel.PasswordHash);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(usuario, isPersistent: true);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(viewModel);
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        UserEntity? usuario = await _userManager.FindByEmailAsync(viewModel.Email);

        if (usuario == null)
        {
            ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
            return View(viewModel);
        }

        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(usuario, viewModel.PasswordHash, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(usuario, isPersistent: viewModel.RememberMe);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult ForgotMyPassword(string message = "")
    {
        ViewBag.Message = message;
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> ForgotMyPassword(ForgotMyPasswordViewModel viewModel)
    {
        string message = "Proceso concluido. Si el email dado corresponde con uno de nuestros usuarios, recibirás un correo con instrucciones para restablecer tu contraseña.";

        ViewBag.Message = message;

        ModelState.Clear(); // Clear model state to prevent validation errors from showing up after redirect

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        UserEntity? usuario = await _userManager.FindByEmailAsync(viewModel.Email);

        if (usuario == null)
        {
            ModelState.AddModelError(string.Empty, "No se encontró un usuario con ese correo electrónico.");
            return View(viewModel);
        }

        // Generate password reset token
        string token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
        // Encode token for use in URL
        string TokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        // Create reset link (you would replace "YourAppUrl" with your actual app URL)
        string resetLink = Url.Action("ResetPassword", "Users", new { token = TokenEncoded }, protocol: Request.Scheme) ?? string.Empty;
        // Send email with reset link
        await _emailService.SendEmailChangePasswordAsync(viewModel.Email, resetLink);

        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult ResetPassword(string token = "")
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            string message = "Token inválido. Por favor, intenta el proceso de restablecimiento de contraseña nuevamente.";
            return RedirectToAction(nameof(ForgotMyPassword), new { message });
        }

        ResetPasswordViewModel viewModel = new()
        {
            TokenReset = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token))
        };

        return View(viewModel);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        UserEntity? usuario = await _userManager.FindByEmailAsync(viewModel.Email);

        if (usuario == null)
        {
            ModelState.AddModelError(string.Empty, "No se encontró un usuario con ese correo electrónico.");
            return View(viewModel);
        }

        IdentityResult result = await _userManager.ResetPasswordAsync(usuario, viewModel.TokenReset, viewModel.PasswordHash);

        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Login));
        }

        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(viewModel);
    }
}
