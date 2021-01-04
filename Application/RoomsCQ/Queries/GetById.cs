using Application.Exeptions;
using Application.Interfaces;
using Application.RoomsCQ.Data;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RoomsCQ.Queries
{
    public class GetById
    {
        public class Query : IRequest<LargeRoomDTO>
        {
            public Guid RoomId { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.RoomId).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Query, LargeRoomDTO>
        {
            DataContext db;
            IMapper mapper;
            iUserAccesor userAccesor;
            public Handler(DataContext dataContext
                           ,iUserAccesor userAccesor
                           ,IMapper mapper)
            {
                this.db = dataContext;
                this.mapper = mapper;
                this.userAccesor = userAccesor;
            }

            public async Task<LargeRoomDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = db.Rooms.Include(x => x.Subscribers).FirstOrDefault(x => x.Id == request.RoomId);
                db.Questions.Where(x => x.RoomId == res.Id).Include(x => x.User).Include(x => x.Likes).Include(x => x.Answer).Load();
                if (res == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Room = "Not found" });
                var room = mapper.Map<Room, LargeRoomDTO>(res);
                room.IsSubscribed = res.Subscribers.Where(x => x.UserId == userAccesor.GetUser().Id).Count() > 0;
                return room;
            }
        }
    }
}
