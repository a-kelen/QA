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
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.AnswersCQ.Queries
{
    class AsnwerList
    {
        public class Query : IRequest<IEnumerable<AnswerDTO>>
        {
            public Guid RoomId { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {

            }
        }
        public class Handler : IRequestHandler<Query, IEnumerable<AnswerDTO>>
        {
            DataContext db;
            IMapper mapper;
            public Handler(DataContext dataContext
                           ,IMapper mapper)
            {
                this.db = dataContext;
                this.mapper = mapper;
            }

            public async Task<IEnumerable<AnswerDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = db.Answers.Where(x => x.Id == request.RoomId).AsEnumerable();
                if( res == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Answers = "Not Found" });
                return mapper.Map<IEnumerable<Answer>, IEnumerable<AnswerDTO>>(res);
            }
        }
    }
}
