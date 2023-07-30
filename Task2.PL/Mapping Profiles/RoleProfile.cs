using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Task2.PL.ViewModels;

namespace Task2.PL.Mapping_Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile() 
        {
            CreateMap<RoleViewModel,IdentityRole>().ForMember(D => D.Name , O => O.MapFrom(S => S.RoleName)).ReverseMap();
        }

    }
}
