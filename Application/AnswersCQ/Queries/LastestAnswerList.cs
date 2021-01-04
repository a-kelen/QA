using Application.AnswersCQ.Data;
using Application.Exeptions;
using Application.Interfaces;
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
    public class LastestAnswerList
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
            iUserAccesor userAccesor;
            public Handler(DataContext dataContext
                           , IMapper mapper
                           , iUserAccesor userAccesor)
            {
                this.db = dataContext;
                this.mapper = mapper;
                this.userAccesor = userAccesor;
            }

            public async Task<IEnumerable<AnswerDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = userAccesor.GetUser();
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
                var userrooms =  db.UserRoom.Where(x => x.UserId == user.Id)
                    .Include(x => x.Room)
                    .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Answer).AsEnumerable().Select(x => x.Room);
                var res = from us in userrooms
                          from q in us.Questions
                          where q. Answer != null
                          select q.Answer;
                if (res == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Answers = "Not Found" });
                return mapper.Map<IEnumerable<Answer>, IEnumerable<AnswerDTO>>(res);
            }
        }
    }
}
