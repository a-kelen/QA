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
    public class OwnList
    {
        public class Query : IRequest<IEnumerable<RoomDTO>>
        {

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
                           ,iUserAccesor userAccesor
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
                await db.Rooms.Where(x => x.UserId == user.Id).Include(x => x.Questions).LoadAsync();
                var res = user.Rooms.AsEnumerable();   
                return mapper.Map<IEnumerable<Room>, IEnumerable<RoomDTO>>(res);
            }
        }
    }
}
