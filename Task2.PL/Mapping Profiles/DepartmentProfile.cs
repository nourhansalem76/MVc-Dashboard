using AutoMapper;
using Task2.DAL.Models;
using Task2.PL.ViewModels;

namespace Task2.PL.Mapping_Profiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile() 
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}
