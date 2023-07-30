using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task2.BLL.Interfaces;
using Task2.DAL.Models;
using Task2.PL.Helpers;
using Task2.PL.ViewModels;

namespace Task2.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //private IEmployeeRepository _EmployeeRepository;
        //private IDepartmentRepository _DepartmentRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            //_EmployeeRepository = EmployeeRepository;
            //_DepartmentRepository = DepartmentRepository;
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            ViewData["Message"] = "Hello View Data";
            ViewBag.message = "Hello View Bag";

            IEnumerable<Employee> Employees;
            if (string.IsNullOrEmpty(SearchValue))

                Employees = await _UnitOfWork.EmployeeRepository.GetAll();

            else

                Employees = _UnitOfWork.EmployeeRepository.GetByName(SearchValue);


            var MapEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);
            return View(MapEmployee);
        }

        [HttpGet] //default
        public IActionResult Create()
        {
            //ViewBag.Departments=  _DepartmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel EmployeeVM)
        {
            if (ModelState.IsValid)
            {
                ///var employee = new Employee()    //Manual Mapping
                ///{
                ///    Name = EmployeeVM.Name,
                ///    Address = EmployeeVM.Address,
                ///    EmaiL = EmployeeVM.EmaiL,
                ///    Salary = EmployeeVM.Salary,
                ///    Age = EmployeeVM.Age,
                ///    DepartmentId = EmployeeVM.DepartmentId,
                ///    IsActive = EmployeeVM.IsActive,
                ///    HireDate = EmployeeVM.HireDate,
                ///    PhoneNumber =EmployeeVM.PhoneNumber
                ///};
                //Employee employee = (Employee)EmployeeVM;

                EmployeeVM.ImageName = await DocumentSettings.UploadFile(EmployeeVM.Image, "img");
                var MapEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                await _UnitOfWork.EmployeeRepository.Add(MapEmployee);
                await _UnitOfWork.Complete();
                //if (count > 0)
                //    TempData["Message"] = "Employee Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(EmployeeVM);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var Employee = await _UnitOfWork.EmployeeRepository.GetById(id.Value);
            if (Employee is null)
            {
                return NotFound();
            }
            var MapEmployee = _mapper.Map<Employee, EmployeeViewModel>(Employee);
            return View(ViewName, MapEmployee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel EmployeeVM)
        {
            if (id != EmployeeVM.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {

                try
                {
                    var MapEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                    _UnitOfWork.EmployeeRepository.Update(MapEmployee);
                    await _UnitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(EmployeeVM);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel EmployeeVM)
        {
            if (id != EmployeeVM.Id)
            {
                return BadRequest();
            }
            try
            {
                var MapEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                _UnitOfWork.EmployeeRepository.Delete(MapEmployee);
                int Checker = await _UnitOfWork.Complete();
                if (Checker > 0)
                {
                    DocumentSettings.DeleteFile(EmployeeVM.ImageName, "img");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(EmployeeVM);
        }
    }
}
