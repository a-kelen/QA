using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.QuestionsCQ.Data;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.QuestionsCQ.Queries
{
    public class ByRoom
    {
        public class Query : IRequest<IEnumerable<QuestionDTO>>
        {
            public Guid RoomId { get; set; }
            
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
            public Handler(DataContext dataContext
                           ,IMapper mapper)
            {
                this.db = dataContext;
                this.mapper = mapper;
            }

            public async Task<IEnumerable<QuestionDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = db.Questions.Where(x => x.RoomId == request.RoomId).AsEnumerable();

                return mapper.Map<IEnumerable<Question>, IEnumerable<QuestionDTO>>(res);
            }
        }
    }
}
