using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameshopWeb.Db;
using Microsoft.EntityFrameworkCore;
using GameshopWeb.JWT;
using System.Security.Claims;

namespace GameshopWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private MyDbContext context;
        private IJwtAuthManager jwtAuthManager;
        public UserController(MyDbContext context, IJwtAuthManager jwtAuthManager)
        {
            this.context = context;
            this.jwtAuthManager = jwtAuthManager;
        }

        [HttpGet("login")]
        public LoginResult Login(string email, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email &&
                u.Password == password);
            if (user == null)
                return null;
            var claims = new Claim[] { new Claim(ClaimTypes.Email, email) };
            var tokens = jwtAuthManager.GenerateTokens(email, claims, DateTime.Now);
            return new LoginResult
            {
                User = user,
                AccessToken = tokens.AccessToken
            };
        }
    }

    public class LoginResult
    {
        public User User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
