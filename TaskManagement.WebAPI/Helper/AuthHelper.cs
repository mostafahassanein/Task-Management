using System.IdentityModel.Tokens.Jwt;
using System;
using TaskManagement.WebAPI.Enums;
using TaskManagement.WebAPI.Models.DTOs;

namespace TaskManagement.WebAPI.Helper
{
    public class AuthHelper
    {
        public static TokenClaimsDto GetTokenClaims(string Token)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(Token);

            return new TokenClaimsDto
            {
                Username = token.Claims.FirstOrDefault(x => x.Type == Enums.Enums.TokenClaims.Username.ToString()).Value,   
            };

        }
    }
}
