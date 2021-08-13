using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tests.Models
{
    public class Question
    {
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public ICollection<Answer> Answers { get; set; }
    }
}
