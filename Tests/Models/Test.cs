using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tests.Models
{
    public class Test
    {
        public string Id { get; set; }
        [Required]
        [MinLength(5)]
        public string Title { get; set; }
        [Required]
        [MinLength(20)]
        public string Description { get; set; }
        [Required]
        public ICollection<Question> Questions { get; set; }
        //[Required]
        public int MinCorrectAnswers { get; set; }
        //[JsonIgnore]
        public TestsUser Author { get; set; }
        public ICollection<AvailableTests> AvailableTests { get; set; }

    }
}
