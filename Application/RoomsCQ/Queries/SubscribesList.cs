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
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RoomsCQ.Queries
{
    public class SubscribesList
    {
        public class Query : IRequest<IEnumerable<RoomDTO>>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Nickname { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {

            }
        }
        public class Handler : IRequestHandler<Query, IEnumerable<RoomDTO>>
        {
            DataContext db;
            iUserAccesor userAccesor;
            IMapper mapper;
            public Handler(DataContext dataContext
                           , iUserAccesor userAccesor
                           , IMapper mapper)
            {
                this.db = dataContext;
                this.mapper = mapper;
                this.userAccesor = userAccesor;
            }

            public async Task<IEnumerable<RoomDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = userAccesor.GetUser();
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
                await db.UserRoom.Where(x => x.UserId == user.Id).Include(x => x.Room).ThenInclude(x => x.Questions).LoadAsync();
                var res = user.Subscribes.AsQueryable().Select(x => x.Room).AsEnumerable();
                return mapper.Map<IEnumerable<Room>, IEnumerable<RoomDTO>>(res);
            }
        }
    }
}
