using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.QuestionsCQ.Data
{
    public class LikeResolver : IValueResolver<Question, QuestionDTO, bool>
    {
        iUserAccesor userAccesor;
        DataContext db;
        public LikeResolver()
        {
            this.db = db;
        }
        public bool Resolve(Question source, QuestionDTO destination, bool destMember, ResolutionContext context)
        {
            var user = userAccesor.GetUser();
            var isSupported = db.Likes.Where(x => x.QuestionId == source.Id && x.UserId == user.Id).Count() > 0;
            return isSupported;
        }
    }
}
