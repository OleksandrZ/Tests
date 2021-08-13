using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string JwtToken { get; set; }
    }
}
