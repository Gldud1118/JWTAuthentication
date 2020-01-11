using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace JWTAuthentication.Areas.Jwt.Controllers
{
    public class AccountController : ApiController
    {
        [AllowAnonymous]
        [Route("api/Account/login")]
        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] LoginRequest login)
        {
            //var loginResponse = new LoginResponse();
            var loginrequest = new LoginRequest()
            {
                UserName = login.UserName.ToLower(),
                Password = login.Password,
                UserRole = login.UserRole

            };

            if(login != null)
            {
                string token = createToken(loginrequest.UserName, loginrequest.UserRole);
                return Ok<string>(token);
            }
            else
            {
                return Unauthorized();
            }


        }

        //토큰 생성하기 
        private string createToken(string username, string userrole)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Role, userrole)
            };

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = "https://localhost:44310",
                Issuer = "https://localhost:44310",
                IssuedAt = DateTime.UtcNow,
                Subject = new ClaimsIdentity(claims, "Bearer"),
                SigningCredentials = signingCredentials,
                NotBefore = DateTime.UtcNow.AddMinutes(-1),
                Expires = DateTime.UtcNow.AddHours(12)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string UserRole { get; set; }
    }

}
