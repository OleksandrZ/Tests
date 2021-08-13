using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models
{
    public class AvailableTests
    {
        public string TestId { get; set; }
        public Test Test { get; set; }
        public string UserId { get; set; }
        public TestsUser User { get; set; }
    }
}
