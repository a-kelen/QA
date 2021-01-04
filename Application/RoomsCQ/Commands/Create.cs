using Application.RoomsCQ.Data;
using Application.UserCQ.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistence;
using Application.Interfaces;
using AutoMapper;
using Application.Exeptions;
using System.Net;
using Domain.Entities;
using System.Linq;

namespace Application.RoomsCQ.Commands
{
    public class Create
    {
        public class Command : IRequest<RoomDTO>
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command, RoomDTO>
        {
            DataContext db;
            IMapper mapper;
            iUserAccesor userAccesor;
            public Handler(DataContext dataContext
                           ,iUserAccesor userAccesor
                           ,IMapper mapper)
            {
                this.db = dataContext;
                this.userAccesor = userAccesor;
                this.mapper = mapper;
            }

            public async Task<RoomDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = userAccesor.GetUser();
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
                var exist = db.Rooms.Where(x => x.Name == request.Name).Count() > 0;
                if(exist)
                    throw new RestException(HttpStatusCode.NotFound, new { Room = "Exist" });
                Room room = new Room
                {
                    Name = request.Name,
                    Description = request.Description,
                    UserId = user.Id
                };
                db.Rooms.Add(room);
                await db.SaveChangesAsync();
                return mapper.Map<Room, RoomDTO>(room);
            }
        }
    }
}
