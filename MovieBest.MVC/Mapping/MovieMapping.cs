using AutoMapper;
using MovieBest.DAL.Models;
using MovieBest.DAL.Entities;

namespace MovieBest.MVC.Mapping
{
    public class MovieMapping : Profile
    {
        public MovieMapping()
        {
            CreateMap<MovieViewModel, Movie>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<Movie, MovieViewModel>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}
