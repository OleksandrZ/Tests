using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models.DTOs
{
    public class TestDto
    {
        public string Id { get; set; }
        [Required]
        [MinLength(5)]
        public string Title { get; set; }
        [Required]
        [MinLength(20)]
        public string Description { get; set; }
        [Required]
        public ICollection<QuestionDto> Questions { get; set; }
        //[Required]
        public int MinCorrectAnswers { get; set; }
    }
}
