using MediatR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Application.UserCQ.Data;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using Persistence;
using Microsoft.AspNetCore.Identity;
using Application.Exeptions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Interfaces;

namespace Application.UserCQ.Commands
{
    public class Register
    {
        public class Command : IRequest<UserDTO>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Nickname { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Firstname).NotEmpty();
                RuleFor(x => x.Lastname).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Nickname).NotEmpty();

            }
        }
        public class Handler: IRequestHandler<Command,UserDTO>
        {
            DataContext db;
            UserManager<User> userManager;
            iJwtGenerator jwtGenerator;
            public Handler(DataContext dataContext, 
                           UserManager<User> userManager,
                           iJwtGenerator jwtGenerator)
            {
                this.db = dataContext;
                this.userManager = userManager;
                this.jwtGenerator = jwtGenerator;
            }

            public async Task<UserDTO> Handle(Command request, CancellationToken cancellationToken)
            {
               
                if (await db.Users.Where(x=> x.Email == request.Email).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });
                if(await db.Users.Where(x => x.Nickname == request.Nickname).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Nickname already exists" });

                var User = new User
                {
                    Firstname = request.Firstname,
                    Lastname = request.Lastname,
                    Email = request.Email,
                    Nickname = request.Nickname,
                    UserName = request.Email
                };
                
                    var result = await userManager.CreateAsync(User, request.Password);


                if (result.Succeeded)
                {
                    return new UserDTO
                    {
                        Id = User.Id,
                        Email = User.Email,
                        Nickname = User.Nickname,
                        Name = User.Firstname + " " + User.Lastname,
                        Token = jwtGenerator.CreateToken(User)
                    };
                }
                throw new Exception("Problem");
            }
        }
    }
}
