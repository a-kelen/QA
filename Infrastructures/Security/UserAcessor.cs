using Application.Exeptions;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Infrastructures.Security
{
    public class UserAcessor : iUserAccesor
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private DataContext db;
        public UserAcessor(IHttpContextAccessor http
            ,DataContext data)
        {
            httpContextAccessor = http;
            db = data;
        }
        public User GetUser()
        {
            string username = httpContextAccessor.HttpContext.User?.Claims?
                                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (username == null)
                throw new RestException(HttpStatusCode.Unauthorized, null);
            User user = db.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, null);
            return user;
        }
    }
}
