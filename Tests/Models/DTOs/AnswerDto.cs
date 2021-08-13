using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models.DTOs
{
    public class AnswerDto
    {
        public string Id { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsAnswer { get; set; }
    }
}
