using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RoomsCQ.Data
{
    public class RoomDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuestionCount { get; set; }
       // public bool IsSubscribed { get; set; }
    }
}
