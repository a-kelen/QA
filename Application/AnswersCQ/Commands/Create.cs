using Application.AnswersCQ.Data;
using Application.Exeptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.AnswersCQ.Commands
{
    public class Create
    {
        public class Command : IRequest<AnswerDTO>
        {
            public string Content { get; set; }
            public Guid QuestionId { get; set; }
         
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Content).NotEmpty();
                RuleFor(x => x.QuestionId).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command, AnswerDTO>
        {
            DataContext db;
            iUserAccesor userAccesor;
            IMapper mapper;
            public Handler(DataContext dataContext
                           ,iUserAccesor userAccesor
                           ,IMapper mapper )
            {
                this.db = dataContext;
                this.userAccesor = userAccesor;
                this.mapper = mapper;
            }

            public async Task<AnswerDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var question = await db.Questions.Include(x => x.Room).FirstOrDefaultAsync(x => x.Id == request.QuestionId);
                if (question == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Question = "Not Found" });
                if(question.Room.UserId != userAccesor.GetUser().Id)
                    throw new RestException(HttpStatusCode.Forbidden, new { User = "Unauthorized" });
                question.State = QuestionState.Passed;
                Answer answer = new Answer
                {
                    Content = request.Content,
                    Question = question,
                };
                question.State = QuestionState.Passed;
                await db.Answers.AddAsync(answer);
                await db.SaveChangesAsync();
                return mapper.Map<Answer, AnswerDTO>(answer);
            }
        }
    }
}
