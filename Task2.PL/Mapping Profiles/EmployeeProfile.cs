using AutoMapper;
using Task2.DAL.Models;
using Task2.PL.ViewModels;

namespace Task2.PL.Mapping_Profiles
{
    public class EmployeeProfile: Profile
    {
        public EmployeeProfile() 
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
