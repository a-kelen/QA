using Application.Exeptions;
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

namespace Application.RoomsCQ.Commands
{
    public class Edit
    {
        public class Command : IRequest<RoomDTO>
        {
            public Guid RoomId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                
            }
        }
        public class Handler : IRequestHandler<Command, RoomDTO>
        {
            DataContext db;
            IMapper mapper;
            public Handler(DataContext dataContext
                           ,IMapper mapper)
            {
                this.db = dataContext;
                this.mapper = mapper;
            }

            public async Task<RoomDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var room = await db.Rooms.FirstOrDefaultAsync(x => x.Id == request.RoomId);
                if (room == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Room = "Not found" });
                room.Name = request.Name ?? room.Name;
                room.Description = request.Description ?? room.Description;
                await db.SaveChangesAsync();
                return mapper.Map<Room, RoomDTO>(room);
            }
        }
    }
}
