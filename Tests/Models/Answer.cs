using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tests.Models
{
    public class Answer
    {
        public string Id { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsAnswer { get; set; }
    }
}
