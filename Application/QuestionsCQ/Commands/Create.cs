using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Exeptions;
using Application.Interfaces;
using Application.QuestionsCQ.Data;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.QuestionsCQ.Commands
{
    public class Create
    {
        public class Command : IRequest<QuestionDTO>
        {
            public Guid RoomId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.RoomId).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command, QuestionDTO>
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

            public async Task<QuestionDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = userAccesor.GetUser();
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Question = "Not Found" });
                var room = await db.Rooms.FindAsync(request.RoomId);
                if (room == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Question = "Not Found" });

                Question question = new Question
                {
                    Room = room,
                    User = user,
                    Description = request.Description,
                    Title = request.Title,
                    State = QuestionState.Active
                };
                await db.Questions.AddAsync(question);
                await db.SaveChangesAsync();
                return mapper.Map<Question, QuestionDTO>(question);
            }
        }
    }
}
