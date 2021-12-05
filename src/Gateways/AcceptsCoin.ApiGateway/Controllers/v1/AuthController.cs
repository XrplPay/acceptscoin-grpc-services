using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AcceptsCoin.ApiGateway.Core.Dtos;
using AcceptsCoin.ApiGateway.Core.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcceptsCoin.ApiGateway.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        public AuthController()
        {

        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] AuhenticateDto auhenticate)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var user = new AuthenticateViewModel();
                    //var user = await _authServices.Authenticate(auhenticate);

                    if (user == null)
                    {
                        return Unauthorized(new WebApiErrorMessageResponse()
                        {
                            Errors = new List<string>() {
                                "Invalid login request "
                            },
                            Success = false
                        });
                    }
                    else
                    {
                        var jwtToken = GenerateJwtToken(user);
                        user.Token = jwtToken;
                        return Ok(user);
                    }

                }
                catch (Exception ex)
                {

                    return BadRequest(new WebApiErrorMessageResponse()
                    {
                        Errors = new List<string>() {
                            "Error in executed"
                    },
                        Success = false
                    });
                }
            }
            else
            {
                return BadRequest(new WebApiErrorMessageResponse()
                {
                    Errors = new List<string>() {
                        "Payload is not valid."
                    },
                    Success = false
                });
            }
        }

        private string GenerateJwtToken(AuthenticateViewModel user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();


            var JWT_SECRET_KEY = Environment.GetEnvironmentVariable("ACCEPTSCOIN_JWT_SECRET_KEY");
            var key = Encoding.ASCII.GetBytes(JWT_SECRET_KEY);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                   // new Claim(JwtRegisteredClaimNames.NameId, user.UserId),
                   // new Claim(JwtRegisteredClaimNames.Email, user.Email),
                   // new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
