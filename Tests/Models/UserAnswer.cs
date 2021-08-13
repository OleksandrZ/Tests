using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models
{
    public class UserAnswer
    {
        public string Id { get; set; }
        public TestsUser User { get; set; }
        public Test Test { get; set; }
        public Question Question { get; set; }
        public Answer Answer { get; set; }
    }
}
