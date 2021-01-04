using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Exeptions;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.QuestionsCQ.Commands
{
    public class SwitchIgnore
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

            public Handler(DataContext dataContext)
            {
                this.db = dataContext;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var question = await db.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId);
                if (question == null)
                    throw new RestException(System.Net.HttpStatusCode.NotFound, new { Question = "Not Found" });
                if (question.State == QuestionState.Active)
                {
                    question.State = QuestionState.Ignored;
                    return false;
                }
                else
                {
                    question.State = QuestionState.Active;
                    return true;
                }

            }
        }
    }
}
