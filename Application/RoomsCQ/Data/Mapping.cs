using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RoomsCQ.Data
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Room, RoomDTO>()
                .ForMember(x => x.QuestionCount, o => o.MapFrom(s => s.Questions.Count));
            CreateMap<Room, LargeRoomDTO>()
                .ForMember(x => x.Id, o => o.MapFrom(s => s.Id))
                .ForMember(x => x.Owner, o => o.MapFrom(s => s.UserId))
                .ForMember(x => x.Name, o => o.MapFrom(s => s.Name))
                .ForMember(x => x.Description, o => o.MapFrom(s => s.Description))
                .ForMember(x => x.Questions, o => o.MapFrom(s => s.Questions))
                .ForMember(x => x.QuestionCount, o => o.MapFrom(s => s.Questions.Count))
                .ForMember(x => x.SubscriberCount, o => o.MapFrom(s => s.Subscribers.Count));
        }
    }
}
