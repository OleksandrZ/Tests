using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models.DTOs
{
    public class QuestionDto
    {
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public ICollection<AnswerDto> Answers { get; set; }
    }
}
