using Application.AnswersCQ.Commands;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.AnswersCQ.Data
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Answer, AnswerDTO>()
                .ForMember(x => x.RoomName, o => o.MapFrom(s => s.Question.Room.Name));
        }
    }
}
