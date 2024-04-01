using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System;
using TaskManagement.WebAPI.Models.Requests;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using TaskManagement.WebAPI.Helper;
using TaskManagement.WebAPI.Models.Responses;

namespace TaskManagement.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly List<string> users = new List<string>() { "mostafa", "adham", "lara" };
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration config, ILogger<AuthController> logger)
        {
            _config = config;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] CreateTokenRequest _request)
        {
            try
            {
                var found = users.Contains(_request.username.ToLower());
                if (!found)
                    return Unauthorized();
                var claims = new[]{
            new Claim("Username",_request.username)};

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds

                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var Token = tokenHandler.WriteToken(token);


                return Ok(new
                {
                    token = Token,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetToken. Timestamp: {Timestamp}", DateTime.UtcNow);
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserInfo()
        {
            try
            {
                var jwt = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (jwt == null)
                    return Unauthorized("Invalid Token");
                var claims = AuthHelper.GetTokenClaims(jwt);
                return Ok(new UserInfoResponse() { username = claims.Username });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UserInfo. Timestamp: {Timestamp}", DateTime.UtcNow);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
