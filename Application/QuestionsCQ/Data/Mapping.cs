using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QuestionsCQ.Data
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Question, QuestionDTO>()
                .ForMember(x => x.Support, o => o.MapFrom(s => s.Likes.Count))
                .ForMember(x => x.IsCompleted, o => o.MapFrom(s => s.State))
                .ForMember(x => x.Answer, o => o.MapFrom(s => s.Answer))
                .ForMember(x => x.Author, o => o.MapFrom(s => s.User.Firstname + " " + s.User.Lastname));
        }
    }
}
