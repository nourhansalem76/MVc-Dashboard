using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;
using Task2.DAL.Models;
using Task2.PL.Helpers;
using Task2.PL.ViewModels;

namespace Task2.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		#region Register
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel RegisterModel)
		{
			if (ModelState.IsValid) //server side validation
			{
				var User = new ApplicationUser()
				{
					FName = RegisterModel.FName,
					LName = RegisterModel.LName,
					UserName = RegisterModel.Email.Split('@')[0],
					Email = RegisterModel.Email,
					IsAgree = RegisterModel.IsAgree,
				};

				var Result = await _userManager.CreateAsync(User, RegisterModel.Password);
				if (Result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
				foreach (var error in Result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(RegisterModel);
		}
		#endregion

		#region Login
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel LoginModel)
		{
			if (ModelState.IsValid)
			{
				var User = await _userManager.FindByEmailAsync(LoginModel.Email);
				if (User is not null)
				{
					var Flag = await _userManager.CheckPasswordAsync(User, LoginModel.Password);
					if (Flag)
					{
						await _signInManager.PasswordSignInAsync(User, LoginModel.Password, LoginModel.RememberMe, false);
						return RedirectToAction("Index", "Home");
					}
					ModelState.AddModelError(string.Empty, "Invalid Password");
				}

				ModelState.AddModelError(string.Empty, "Email is not existed!");
			}
			return View(LoginModel);
		}

		#endregion

		#region Sign Out
		public new async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
		#endregion

		#region ForgetPassword

		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var User = await _userManager.FindByEmailAsync(model.Email);
				
				if(User is not null)
				{
					var Token = await _userManager.GeneratePasswordResetTokenAsync(User);
					var ResetPassword= Url.Action("ResetPassword", "Account", new { email = User.Email ,token= Token },Request.Scheme);
					var Email = new Email()
					{
						Subject = "Reset Password",
						To = model.Email,
						Body = ResetPassword

                    };
					EmailSettings.SendEmail(Email);
				 return	RedirectToAction(nameof(CheckInbox));
				}
				ModelState.AddModelError(string.Empty, "Email is not existed!");

			}
			return View(model);
		}

		#endregion

		public IActionResult CheckInbox()
		{
			return View();
		}

		#region Reset Password
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if(ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				string token = TempData["token"] as string; 
				var User = await _userManager.FindByEmailAsync(email);

				var Result=await _userManager.ResetPasswordAsync(User, token, model.NewPassword);
				if (Result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
				foreach (var error in Result.Errors)
				{
					ModelState.AddModelError(string.Empty,error.Description);
				}

			}
			return View(model);
		}
		#endregion
	}
}
