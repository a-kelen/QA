using Application.Exeptions;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.QuestionsCQ.Commands
{
    public class Ignore
    {
        public class Command : IRequest<bool>
        {
            public Guid QuestionId { get; set; }

        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.QuestionId).NotEmpty();
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
                var question = await db.Questions.FindAsync(request.QuestionId);
                if (question == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Question = "Not Found" });
                question.State = Domain.Entities.QuestionState.Ignored;
                
                await db.SaveChangesAsync();
                return false;
            }
        }
    }
}
