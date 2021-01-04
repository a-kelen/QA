using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public long Ranking { get; set; }
        public QuestionState State { get; set; }
        public Answer Answer { get; set; }
        public ICollection<Like> Likes { get; set; }
        public DateTime Created { get; set; }
    }
    public enum QuestionState
    {
        Active,
        Passed,
        Ignored
    }

}
