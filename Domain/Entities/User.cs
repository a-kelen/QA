using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Nickname { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<UserRoom> Subscribes { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Question> Questions { get; set; }
        public DateTime Created { get; set; }

    }
}
