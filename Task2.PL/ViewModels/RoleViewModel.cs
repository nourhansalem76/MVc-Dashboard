using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Task2.PL.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public RoleViewModel() 
        { 
            Id= Guid.NewGuid().ToString();
        }
    }
}
