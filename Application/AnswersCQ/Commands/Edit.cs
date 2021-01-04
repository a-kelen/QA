using Application.AnswersCQ.Data;
using Application.Exeptions;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.AnswersCQ.Commands
{
    public class Edit
    {
        public class Command : IRequest<AnswerDTO>
        {
            public string Content { get; set; }
            public Guid AnswerId { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                
            }
        }
        public class Handler : IRequestHandler<Command, AnswerDTO>
        {
            DataContext db;
            IMapper mapper;

            public Handler(DataContext dataContext
                            ,IMapper mapper)
            {
                this.db = dataContext;
                this.mapper = mapper;
            }

            public async Task<AnswerDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var answer = db.Answers.FirstOrDefault(x => x.Id == request.AnswerId);
                if(answer == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Answer = "Not Found" });

                answer.Content = request.Content ?? answer.Content;
                await db.SaveChangesAsync();
                return mapper.Map<Answer,AnswerDTO>(answer);
            }
        }
    }
}
