using System;
using AutoMapper;
using dotNetLab2.Models;
using dotNetLab2.ViewModels;

namespace dotNetLab2
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieViewModel>();//.ReverseMap();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<Movie, MovieWithCommentsViewModel>();
        }
    }
}
