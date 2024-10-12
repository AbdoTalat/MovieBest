using AutoMapper;
using MovieBest.DAL.Models;
using MovieBest.DAL.Entities;

namespace MovieBest.MVC.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());


        }
    }
}
