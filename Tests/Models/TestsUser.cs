using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models
{
    public class TestsUser : IdentityUser
    {
        public ICollection<AvailableTests> AvailableTests { get; set; }
        public ICollection<PassedTests> PassedTests { get; set; }
    }
}
