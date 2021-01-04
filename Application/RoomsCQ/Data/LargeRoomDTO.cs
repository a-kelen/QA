using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RoomsCQ.Data
{
    public class LargeRoomDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuestionCount { get; set; }
        public int SubscriberCount { get; set; }
        public IEnumerable<Application.QuestionsCQ.Data.QuestionDTO> Questions { get; set; }
        public IEnumerable<Answer> Answers  { get; set; }
        public Guid Owner { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
