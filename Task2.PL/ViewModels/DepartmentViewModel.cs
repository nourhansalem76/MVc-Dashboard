﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using Task2.DAL.Models;

namespace Task2.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is required!!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required!!")]
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }


        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>(); 
    }
}
