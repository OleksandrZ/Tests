using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models
{
    public class PassedTests
    {
        public string Id { get; set; }
        public string TestId { get; set; }
        public Test Test { get; set; }
        public int TotalScore { get; set; }
        public ICollection<UserAnswer> Answers { get; set; }
    }
}
