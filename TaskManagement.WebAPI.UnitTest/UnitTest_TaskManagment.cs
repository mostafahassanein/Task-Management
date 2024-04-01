using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Application.Interfaces;
using TaskManagement.WebAPI.Controllers;
using TaskManagement.WebAPI.Models.Requests;
using TaskManagement.WebAPI.Models.Responses;

namespace TaskManagement.WebAPI.UnitTest
{
    [TestFixture]
    public class UnitTest_TaskManagment
    {

        private TaskManagmentController _controller;
        private Mock<ITaskManagementService> _mockTaskManagementService;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<TaskManagmentController>> _mockLogger;
        private readonly string validToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJVc2VybmFtZSI6Im1vc3RhZmEiLCJuYmYiOjE3MTE5OTI5ODAsImV4cCI6MTcxMjA3OTM4MCwiaWF0IjoxNzExOTkyOTgwfQ.XqD6MCAGu-D5E35NZa0jxtNEzokNjSn4PMtzPi-GGddFpS7LCBuiTZnQ8Ek4F38lEHWiCgkWC2bhxE5-Eij9Bg";

        [SetUp]
        public void Setup()
        {
            _mockTaskManagementService = new Mock<ITaskManagementService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<TaskManagmentController>>();

            _controller = new TaskManagmentController(
                _mockTaskManagementService.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        [Test]
        public async Task GetTask_WithValidTaskId_ReturnsOk()
        {
            var taskId = "1";
            var jwt = validToken;
            var taskModel = new TaskManagement.Entities.Tasks();
            _mockTaskManagementService.Setup(x => x.GetTask(taskId)).ReturnsAsync(taskModel);
            _controller.ControllerContext = GetControllerContextWithAuthorizationHeader(jwt);

            var result = await _controller.GetTask(taskId) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(taskModel, result.Value);
        }

        [Test]
        public async Task GetAllTasks_ReturnsOk()
        {
            var jwt = validToken;
            var taskModels = new List<TaskManagement.Entities.Tasks>(); // Assuming TaskModel is defined in YourNamespace.Models
            _mockTaskManagementService.Setup(x => x.GetAllsTasks()).ReturnsAsync(taskModels);
            _controller.ControllerContext = GetControllerContextWithAuthorizationHeader(jwt);

            var result = await _controller.GetAllTasks() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        

        private ControllerContext GetControllerContextWithAuthorizationHeader(string jwt, ClaimsIdentity claims = null)
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

            if (claims != null)
            {
                httpContext.User = new ClaimsPrincipal(claims);
            }

            return new ControllerContext { HttpContext = httpContext };
        }

        [Test]
        public async Task UpdateTask_WithValidRequest_ReturnsOk()
        {
            var jwt = validToken;
            string resultStr = "";
            var request = new AddTaskRequest { username = "mostafa" };
            var taskModel = new TaskManagement.Entities.Tasks(); 
            _mockTaskManagementService.Setup(x => x.UpdateTask(taskModel)).ReturnsAsync(resultStr);
            _controller.ControllerContext = GetControllerContextWithAuthorizationHeader(jwt);

            var result = await _controller.UpdateTask(request) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task RemoveTask_WithValidRequest_ReturnsOk()
        {
            var jwt = validToken;
            string resultStr = "";
            var request = new RemoveTaskRequest { taskId=2 };
            var taskModel = new TaskManagement.Entities.Tasks();
            _mockTaskManagementService.Setup(x => x.RemoveTask(request.taskId.ToString())).ReturnsAsync(resultStr);
            _controller.ControllerContext = GetControllerContextWithAuthorizationHeader(jwt);

            var result = await _controller.RemoveTask(request) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }



    }
}
