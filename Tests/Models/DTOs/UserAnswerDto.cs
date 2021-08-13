using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models.DTOs
{
    public class UserAnswerDto
    {
        public string TestId { get; set; }
        public string QuestionId { get; set; }
        public string AnswerId { get; set; }
    }
}
