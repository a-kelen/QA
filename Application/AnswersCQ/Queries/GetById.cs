using Application.AnswersCQ.Data;
using Application.Exeptions;
using AutoMapper;
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

namespace Application.AnswersCQ.Queries
{
    public class GetById
    {
        public class Query : IRequest<AnswerDTO>
        {
            public Guid Id { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {

            }
        }
        public class Handler : IRequestHandler<Query, AnswerDTO>
        {
            DataContext db;
            IMapper mapper;
            public Handler(DataContext dataContext
                           ,IMapper mapper)
            {
                this.db = dataContext;
                this.mapper = mapper;
            }

            public async Task<AnswerDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = await db.Answers.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (res == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Answer = "Not Found" });
                return mapper.Map<Answer, AnswerDTO>(res);
            }
        }
    }
}
