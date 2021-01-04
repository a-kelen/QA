using Application.AnswersCQ.Data;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.QuestionsCQ.Data
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int Support { get; set; }
        public bool IsSupported { get; set; }
        public QuestionState IsCompleted { get; set; }
        public AnswerDTO Answer { get; set; }
    }
}
