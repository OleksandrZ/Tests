using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models.DTOs
{
    public class TestResultDto
    {
        public int MinCorrectAnswers { get; set; }
        public int MaxCorrectAnswers { get; set; }
        public int TotalCorrectAnswers { get; set; }
    }
}
