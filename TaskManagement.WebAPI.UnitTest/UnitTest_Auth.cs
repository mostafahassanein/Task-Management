using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.WebAPI.Controllers;
using TaskManagement.WebAPI.Models.Requests;
using TaskManagement.WebAPI.Models.Responses;

namespace TaskManagement.WebAPI.UnitTest
{
    [TestFixture]
    public class AuthControllerTests
    {
        private AuthController _authController;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<ILogger<AuthController>> _mockLogger;
        private readonly string secretKey = "JWTAuthenticationHIGHsecuredPasswordVVVp1OH7XzyrSGDGTRYdsnsdfyHHLIOSSJSJSMRsfbvMDJdQwrRdRYyUIfsAaTuOh";
        private readonly string user = "mostafa";

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _authController = new AuthController(_mockConfiguration.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetToken_WithValidCredentials_ReturnsOk()
        {
            var validRequest = new CreateTokenRequest { username = user };
            _mockConfiguration.Setup(x => x["JWT:Secret"]).Returns(secretKey);

            var result = await _authController.GetToken(validRequest) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Value);
            Assert.IsNotNull(result.Value.GetType() == typeof(string)); // Assuming your token is returned as a string
        }

        [Test]
        public async Task UserInfo_WithValidToken_ReturnsOk()
        {
            var validToken = GenerateValidToken();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = $"Bearer {validToken}";

            _authController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await _authController.UserInfo() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(typeof(UserInfoResponse), result.Value.GetType());
            var userInfoResponse = result.Value as UserInfoResponse;
            Assert.AreEqual(user, userInfoResponse.username); 
        }

        private string GenerateValidToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Username", user) }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}