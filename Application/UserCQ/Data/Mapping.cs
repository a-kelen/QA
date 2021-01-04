using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UserCQ.Data
{
    public class Mapping :Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDTO>()
                .ForMember(x => x.Id, o => o.MapFrom(s => s.Id))
                .ForMember(x => x.Name, o => o.MapFrom(s => s.Firstname + " " + s.Lastname))
                .ForMember(x => x.Nickname, o => o.MapFrom(s => s.Nickname))
                .ForMember(x => x.Email, o => o.MapFrom(s => s.Email));
        }
    }
}
