using Application.Exeptions;
using Application.Interfaces;
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
    public class Switch
    {
        public class Command : IRequest<bool>
        {
            public Guid RoomId{ get; set; }
            
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.RoomId).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command, bool>
        {
            DataContext db;
            iUserAccesor userAccesor;
            public Handler(DataContext dataContext
                           ,iUserAccesor userAccesor)
            {
                this.db = dataContext;
                this.userAccesor = userAccesor;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = userAccesor.GetUser();
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
                var room = db.Rooms.Include(x => x.Subscribers).FirstOrDefault(x => x.Id == request.RoomId);
                if (room == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Room = "Not found" });
                var userRoom = room.Subscribers.FirstOrDefault(x => x.RoomId == request.RoomId && x.UserId == user.Id);
                if (userRoom == null)
                {
                    room.Subscribers.Add(new UserRoom { User = user, Room = room });
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    db.UserRoom.Remove(userRoom);
                    await db.SaveChangesAsync();
                    return false;
                }

                
            }
        }
    }
}
