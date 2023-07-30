using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Task2.DAL.Models;
using Task2.PL.ViewModels;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Task2.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var Roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName= R.Name,
                   
                }).ToListAsync();
                return View(Roles);
            }
            else
            {
                var Roles = await _roleManager.FindByNameAsync(name);
               if(Roles is not null)
                {
                    var MappedRole = new RoleViewModel()
                    {
                        Id = Roles.Id,
                        RoleName = Roles.Name,

                    };
                    return View(new List<RoleViewModel>() { MappedRole });
                }

                return View(Enumerable.Empty<RoleViewModel>());
            }

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleView)
        {
            if(ModelState.IsValid)
            {
                var MapRole= _mapper.Map<RoleViewModel,IdentityRole>(roleView);
                await _roleManager.CreateAsync(MapRole);
                return RedirectToAction(nameof(Index));
            }
            return View(roleView);
        }

        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var Role = await _roleManager.FindByIdAsync(id);
            if (Role is null)
            {
                return NotFound();
            }
            var MapRole = _mapper.Map<IdentityRole, RoleViewModel>(Role);
            return View(ViewName, MapRole);
        }

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel roleView)
        {
            if (id != roleView.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {

                try
                {
                    var Role = await _roleManager.FindByIdAsync(id);

                    Role.Name = roleView.RoleName;

                   


                    await _roleManager.UpdateAsync(Role);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(roleView);
        }
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel roleView)
        {
            if (id != roleView.Id)
            {
                return BadRequest();
            }
            try
            {
                var Role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(Role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(roleView);
        }
    }
}
