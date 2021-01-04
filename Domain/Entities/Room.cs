using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Room
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long SubscriberCount { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Question> Questions { get; set; }
        public string Description { get; set; }
        public ICollection<UserRoom> Subscribers { get; set; }
        public DateTime Created { get; set; }
    }
}
