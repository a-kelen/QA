using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Exeptions;
using Application.Interfaces;
using Application.UserCQ.Data;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence;
namespace Application.UserCQ.Commands
{
    public class ChangeProfile
    {
        public class Command : IRequest<UserDTO>
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {

            }
        }
        public class Handler : IRequestHandler<Command, UserDTO>
        {
            DataContext db;
            IMapper mapper;
            iUserAccesor userAccesor;
            public Handler(DataContext dataContext
                           ,IMapper mapper
                           ,iUserAccesor userAccesor)
            {
                this.db = dataContext;
                this.mapper = mapper;
                this.userAccesor = userAccesor;
            }

            public async Task<UserDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = userAccesor.GetUser();
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
                user.Firstname = request.Firstname ?? user.Firstname;
                user.Lastname = request.Lastname ?? user.Lastname;
                await db.SaveChangesAsync();
                return mapper.Map<User, UserDTO>(user);
            }
        }
    }
}
