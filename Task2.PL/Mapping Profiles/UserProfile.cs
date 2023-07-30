using AutoMapper;
using Task2.DAL.Models;
using Task2.PL.ViewModels;

namespace Task2.PL.Mapping_Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}
