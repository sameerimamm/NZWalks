using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly ITokenRepository tokenRepository;

        //POST: /api/Auth/Register

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration,
            ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // Add roles to this user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login.");
                    }

                }
            }

            return BadRequest("Something went wrong");



        }


        //POST: /api/auth/Login

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user == null)
            {
                return BadRequest("User not found");


            }

            var checkedPassword = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!checkedPassword)
            {
                return BadRequest("Incorrect Password");
            }
            //start
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
            //  configuration["Jwt:Issuer"],
            //  null,
            //  expires: DateTime.Now.AddMinutes(120),
            //  signingCredentials: credentials);

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var jwtToken = tokenHandler.WriteToken(token);
            //end
            var roles = await userManager.GetRolesAsync(user);


            Console.WriteLine(user.ToString());
            Console.WriteLine(user.Email.ToString());

            if (roles == null)
            {
                return BadRequest("No roles found");

               
            }


            var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());

            var token = new LoginResponse
            {
                jwtToken = jwtToken,
                Roles = roles.ToArray(),
            };




            return Ok(token);


        }




    }

}
