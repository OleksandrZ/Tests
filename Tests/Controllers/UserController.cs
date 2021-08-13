using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.Models;
using Tests.Models.DTOs;
using Tests.Utils.Interfaces;

namespace Tests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<TestsUser> userManager;
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly SignInManager<TestsUser> signInManager;

        public UserController(UserManager<TestsUser> userManager, IJwtTokenGenerator jwtTokenGenerator, SignInManager<TestsUser> signInManager)
        {
            this.userManager = userManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.signInManager = signInManager;
        }

        //api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto login)
        {
            var user = await userManager.FindByEmailAsync(login.Email);

            if(user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, false);
               
                if (result.Succeeded)
                {
                    UserDto userDto = new UserDto()
                    {
                        Username = user.UserName,
                        JwtToken = await jwtTokenGenerator.CreateToken(user)
                    };

                    return Ok(userDto);
                }
                
                return Unauthorized();
            }
            return Unauthorized();
        }

        //api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto register)
        {
            var userExists = await userManager.FindByEmailAsync(register.Email);
            if (userExists != null)
                return BadRequest("User with such email already registered");

            userExists = await userManager.FindByNameAsync(register.Username);

            if (userExists != null)
                return BadRequest("User with such username already registered");

            TestsUser user = new TestsUser()
            {
                UserName = register.Username,
                Email = register.Email
            };

            var result = await userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
