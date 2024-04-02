using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Application.Interfaces;
using TaskManagement.Entities;
using TaskManagement.WebAPI.Controllers;
using TaskManagement.WebAPI.Hubs;
using TaskManagement.WebAPI.Models.DTOs;
using TaskManagement.WebAPI.Models.Requests;
using TaskManagement.WebAPI.Models.Responses;

namespace TaskManagement.WebAPI.UnitTest
{
    [TestFixture]
    public class UnitTest_Chat
    {
        private ChatController _controller;
        private Mock<IMapper> _mockMapper;
        private Mock<IChatHistoryService> _mockChatHistoryService;
        private Mock<IHubContext<ChatHub>> _mockHubContext;
        private Mock<ILogger<ChatController>> _mockLogger;
        private readonly string validToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJVc2VybmFtZSI6Im1vc3RhZmEiLCJuYmYiOjE3MTE5OTI5ODAsImV4cCI6MTcxMjA3OTM4MCwiaWF0IjoxNzExOTkyOTgwfQ.XqD6MCAGu-D5E35NZa0jxtNEzokNjSn4PMtzPi-GGddFpS7LCBuiTZnQ8Ek4F38lEHWiCgkWC2bhxE5-Eij9Bg";

        [SetUp]
        public void Setup()
        {
            _mockMapper = new Mock<IMapper>();
            _mockChatHistoryService = new Mock<IChatHistoryService>();
            _mockHubContext = new Mock<IHubContext<ChatHub>>();
            _mockLogger = new Mock<ILogger<ChatController>>();

            _controller = new ChatController(
                _mockMapper.Object,
                _mockChatHistoryService.Object,
                _mockHubContext.Object,
                _mockLogger.Object
            );
        }

        private ControllerContext GetControllerContextWithAuthorizationHeader(string jwt)
        {
            var headerDictionary = new HeaderDictionary
            {
                { HeaderNames.Authorization, $"Bearer {jwt}" }
            };

            var httpContext = new DefaultHttpContext { RequestServices = new ServiceCollection().BuildServiceProvider() };
            foreach (var header in headerDictionary)
            {
                httpContext.Request.Headers.Add(header);
            }

            return new ControllerContext { HttpContext = httpContext };
        }

        [Test]
        public async Task GetFriendsList_WithValidToken_ReturnsOk()
        {
            var jwt = validToken;
            _controller.ControllerContext = GetControllerContextWithAuthorizationHeader(jwt);           
            var result = await _controller.GetFriendsList();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult<IEnumerable<UserInfoResponse>>>(result);
            var okResult = result as ActionResult<IEnumerable<UserInfoResponse>>;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<OkObjectResult>(okResult.Result);
            var okObjectResult = okResult.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult.Value);
        }

        [Test]
        public async Task GetChatHistory_WithValidRequest_ReturnsOk()
        {
            var request = new GetChatHistoryRequest { recipient = "lara" };
            var jwt = validToken;
            var claims = new ClaimsIdentity(new[] { new Claim("Username", "mostafa") });
            _controller.ControllerContext = GetControllerContextWithAuthorizationHeader(jwt);

            var chatHistory = new List<ChatMessage>(); 
            _mockChatHistoryService.Setup(x => x.GetChatHistory()).ReturnsAsync(chatHistory);

            var result = await _controller.GetChatHistory(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult<List<GetChatHistoryResponse>>>(result);
            var actionResult = result as ActionResult<List<GetChatHistoryResponse>>;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
            var okObjectResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
        }
    }
}
