using Application.Exeptions;
using Application.Interfaces;
using Application.UserCQ.Data;
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

namespace Application.UserQA.Queries
{
    public class Get
    {
        public class Query : IRequest<UserDTO>
        {

        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {

            }
        }
        public class Handler : IRequestHandler<Query, UserDTO>
        {
            DataContext db;
            iUserAccesor userAccesor;
            IMapper mapper;
            public Handler(DataContext dataContext
                           ,iUserAccesor userAccesor
                           ,IMapper mapper)
            {
                this.db = dataContext;
                this.userAccesor = userAccesor;
                this.mapper = mapper;
            }

            public async Task<UserDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = userAccesor.GetUser();
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
                return mapper.Map<User, UserDTO>(user);
            }
        }
    }
}
