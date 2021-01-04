using System;
using System.Collections.Generic;
using System.Text;

namespace Application.AnswersCQ.Data
{
    public class AnswerDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string RoomName { get; set; }
    }
}
