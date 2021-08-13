using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models.DTOs
{
    public class UserAnswerDto
    {
        [Required]
        public string TestId { get; set; }
        [Required]
        public string QuestionId { get; set; }
        [Required]
        public string AnswerId { get; set; }
    }
}
