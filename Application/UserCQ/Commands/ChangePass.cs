using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Exeptions;
using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;
namespace Application.UserCQ.Commands
{
    public class ChangePass
    {
        public class Command : IRequest<bool>
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.OldPassword).NotEmpty();
                RuleFor(x => x.NewPassword).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command, bool>
        {
            UserManager<User> manager;
            iUserAccesor userAccesor;
            IPasswordHasher<User> hasher;
            IPasswordValidator<User> validator;
            public Handler(UserManager<User> manager
                           ,iUserAccesor userAccesor
                           ,IPasswordValidator<User> validator
                           ,IPasswordHasher<User> hasher
                           )
            {
                this.userAccesor = userAccesor;
                this.manager = manager;
                this.validator = validator;
                this.hasher = hasher;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = userAccesor.GetUser();
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
                var pass1 = await manager.CheckPasswordAsync(user, request.OldPassword);
                if (pass1)
                {
                    IdentityResult result = await validator.ValidateAsync(manager, user, request.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = hasher.HashPassword(user, request.NewPassword);
                        await manager.UpdateAsync(user);
                        return true;
                    }
                }
                return false;

            }
        }
    }
}
