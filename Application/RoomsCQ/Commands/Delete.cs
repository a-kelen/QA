using Application.Exeptions;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RoomsCQ.Commands
{
    public class Delete
    {
        public class Command : IRequest<bool>
        {
            public Guid RoomId { get; set; }
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
                           , iUserAccesor userAccesor)
            {
                this.db = dataContext;
                this.userAccesor = userAccesor;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = userAccesor.GetUser();
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
                var room = await db.Rooms.FirstOrDefaultAsync(x => x.Id == request.RoomId && x.UserId == user.Id);
                if (room == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Room = "Not found" });
                db.Rooms.Remove(room);
                return await db.SaveChangesAsync() > 0;
            }
        }
    }
}
