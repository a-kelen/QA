using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistence;
namespace Application.UserCQ.Commands
{
    public class AddAvatar
    {
        public class Command : IRequest<string>
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

            }
        }
        public class Handler : IRequestHandler<Command, string>
        {
            DataContext db;

            public Handler(DataContext dataContext)
            {
                this.db = dataContext;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new Exception("Problem");
            }
        }
    }
}
