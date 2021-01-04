using Application.Exeptions;
using Application.Interfaces;
using Domain.Entities;
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

namespace Application.LikesCQ.Commands
{
    public class Switch
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
            public Handler(DataContext dataContext, iUserAccesor userAccesor)
            {
                this.db = dataContext;
                this.userAccesor = userAccesor;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = userAccesor.GetUser();
                var question = await db.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId);
                if(user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not Found" });
                if (question == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Question = "Not Found" });
                var exist = await db.Likes.FirstOrDefaultAsync(x => x.QuestionId == question.Id && x.UserId == user.Id);
                if (exist == null)
                {
                    db.Likes.Add(new Like { Question = question, User = user });
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    db.Likes.Remove(exist);
                    await db.SaveChangesAsync();
                    return false;
                }

            }
        }
    }
}
