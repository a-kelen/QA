using Application.Exeptions;
using Application.Interfaces;
using Application.UserCQ.Data;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserCQ.Commands
{
    public class Login
    {
        public class Command : IRequest<UserDTO>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
               
            }
        }
        public class Handler : IRequestHandler<Command, UserDTO>
        {
            DataContext db;
            UserManager<User> userManager;
            iJwtGenerator jwtGenerator;
            SignInManager<User> signInManager;
            public Handler(DataContext dataContext,
                           UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           iJwtGenerator jwtGenerator)
            {
                this.db = dataContext;
                this.userManager = userManager;
                this.jwtGenerator = jwtGenerator;
                this.signInManager = signInManager;
            }

            public async Task<UserDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                
                var user = await userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized, "asdadasd");
                var res = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if(res.Succeeded)
                {
                    return new UserDTO
                    {
                        Name = user.Firstname + " " + user.Lastname,
                        Token = jwtGenerator.CreateToken(user),
                        Nickname = user.Nickname,
                        Email = user.Email
                    };
                }
                throw new RestException(HttpStatusCode.Unauthorized, null);
                      
            }
        }
    }
}
