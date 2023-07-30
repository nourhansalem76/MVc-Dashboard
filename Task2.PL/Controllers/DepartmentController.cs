using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task2.BLL.Interfaces;
using Task2.DAL.Models;
using Task2.PL.ViewModels;

namespace Task2.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        //private IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            //_departmentRepository = departmentRepository;
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _UnitOfWork.DepartmentRepository.GetAll();
            var MapDepartment = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(MapDepartment);
        }

        [HttpGet] //default
        public IActionResult Create()  
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel DepartmentVM)
        {
            if (ModelState.IsValid)
            {
                var MapDepartment = _mapper.Map<DepartmentViewModel, Department>(DepartmentVM);
                await _UnitOfWork.DepartmentRepository.Add(MapDepartment);
                await _UnitOfWork.Complete();
                //if (count > 0)
                //    TempData["Message"] = "Department created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(DepartmentVM);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department = await _UnitOfWork.DepartmentRepository.GetById(id.Value);

            if (department is null)
            {
                return NotFound();
            }
            var MapDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(ViewName, MapDepartment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
            ///if (id is null)
            ///{
            ///    return BadRequest();
            ///}
            ///var department = _departmentRepository.GetById(id.Value);
            ///if (department is null)
            ///{
            ///    return NotFound();
            ///}
            ///return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, DepartmentViewModel DepartmentVM)
        {
            if (id != DepartmentVM.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {

                try
                {
                    var MapDepartment = _mapper.Map<DepartmentViewModel, Department>(DepartmentVM);
                    _UnitOfWork.DepartmentRepository.Update(MapDepartment);
                    await _UnitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(DepartmentVM);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentViewModel DepartmentVM)
        {
            if (id != DepartmentVM.Id)
            {
                return BadRequest();
            }
            try
            {
                var MapDepartment = _mapper.Map<DepartmentViewModel, Department>(DepartmentVM);
                _UnitOfWork.DepartmentRepository.Delete(MapDepartment);
                await _UnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(DepartmentVM);
        }
    }
}
