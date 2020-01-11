using JWTAuthentication.Areas.Jwt.AuthFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JWTAuthentication.Areas.Jwt.Controllers
{
    public class TestController : ApiController
    {
        // GET: api/Test/5

        [JwtAuthenticationFilter]
        [Authorize(Roles = "Admin")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
