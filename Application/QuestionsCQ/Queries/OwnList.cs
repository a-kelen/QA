using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.QuestionsCQ.Queries
{
    public class OwnList
    {
        public class Query : IRequest<IEnumerable<QuestionDTO>>
        {

        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {

            }
        }
        public class Handler : IRequestHandler<Query, IEnumerable<QuestionDTO>>
        {
            DataContext db;
            IMapper mapper;
            iUserAccesor userAccesor;
            public Handler(DataContext dataContext
                           ,IMapper mapper
                           , iUserAccesor userAccesor)
            {
                this.db = dataContext;
                this.mapper = mapper;
                this.userAccesor = userAccesor;
            }

            public async Task<IEnumerable<QuestionDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = userAccesor.GetUser();
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
                await db.Questions.Where(x => x.UserId == user.Id).Include(x => x.Likes).LoadAsync();
                var res = user.Questions;
                return mapper.Map<IEnumerable<Question>, IEnumerable<QuestionDTO>>(res);
            }
        }
    }
}
