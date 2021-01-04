using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.QuestionsCQ.Data;
using FluentValidation;
using MediatR;
using Persistence;
namespace Application.QuestionsCQ.Queries
{
    public class QuestionList
    {
        public class Query : IRequest<IEnumerable<QuestionDTO>>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Nickname { get; set; }
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

            public Handler(DataContext dataContext)
            {
                this.db = dataContext;
            }

            public async Task<IEnumerable<QuestionDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new Exception("Problem");
            }
        }
    }
}
