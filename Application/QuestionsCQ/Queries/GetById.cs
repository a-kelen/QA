using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Exeptions;
using Application.QuestionsCQ.Data;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.QuestionsCQ.Queries
{
    public class GetById
    {
        public class Query : IRequest<QuestionDTO>
        {
            public  Guid QuestionId { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.QuestionId).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Query, QuestionDTO>
        {
            DataContext db;
            IMapper mapper;
            public Handler(DataContext dataContext
                           ,IMapper mapper)
            {
                this.db = dataContext;
                this.mapper = mapper;
            }

            public async Task<QuestionDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = await db.Questions.FirstOrDefaultAsync(x => x.Id == request.QuestionId);
                if (res == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Question = "Not Found" });
                return mapper.Map<Question, QuestionDTO>(res);
            }
        }
    }
}
