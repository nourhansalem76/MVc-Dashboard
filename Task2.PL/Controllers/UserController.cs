using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task2.DAL.Models;
using Task2.PL.Helpers;
using Task2.PL.ViewModels;

namespace Task2.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                var Users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FName = U.FName,
                    LName = U.LName,
                    Email = U.Email,
                    PhoneNumber = U.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToListAsync();
                return View(Users);
            }
            else
            {
                var User = await _userManager.FindByEmailAsync(email);
                var MappedUser = new UserViewModel()
                {
                    Id = User.Id,
                    FName = User.FName,
                    LName = User.LName,
                    Email = User.Email,
                    PhoneNumber = User.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(User).Result
                };
                return View(new List<UserViewModel>() { MappedUser });
            }

        }

        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var User = await _userManager.FindByIdAsync(id);
            if (User is null)
            {
                return NotFound();
            }
            var MapUser = _mapper.Map<ApplicationUser, UserViewModel>(User);
            return View(ViewName, MapUser);
        }

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel userView)
        {
            if (id != userView.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {

                try
                {
                    var User = await _userManager.FindByIdAsync(id);
                    
                    User.FName = userView.FName;
                    User.LName = userView.LName;
                    User.PhoneNumber = userView.PhoneNumber;
                    
                   
                    await _userManager.UpdateAsync(User);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(userView);
        }
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel userView)
        {
            if (id != userView.Id)
            {
                return BadRequest();
            }
            try
            {
                var User = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(User);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(userView);
        }

    }
}
