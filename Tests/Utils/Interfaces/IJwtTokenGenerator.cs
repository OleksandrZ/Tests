using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.Models;

namespace Tests.Utils.Interfaces
{
     public interface IJwtTokenGenerator
    {
        public Task<string> CreateToken(TestsUser user);
    }
}
