using Application.RoomsCQ.Data;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.RoomsCQ.Queries
{
    public class Search
    {
        public class Query : IRequest<IEnumerable<RoomDTO>>
        {
            public string Name { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Query, IEnumerable<RoomDTO>>
        {
            DataContext db;
            IMapper mapper;
            public Handler(DataContext dataContext
                           ,IMapper mapper)
            {
                this.db = dataContext;
                this.mapper = mapper;
            }

            public async Task<IEnumerable<RoomDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var res = db.Rooms.Where(x => x.Name.Contains(request.Name)).Include(x => x.Questions);
                return mapper.Map<IEnumerable<Room>, IEnumerable<RoomDTO>>(res);
            }
        }
    }
}
