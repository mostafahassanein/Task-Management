using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using TaskManagement.Application.Interfaces;
using TaskManagement.WebAPI.Helper;
using TaskManagement.WebAPI.Models.Requests;
using TaskManagement.WebAPI.Models.Responses;

namespace TaskManagement.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class TaskManagmentController : ControllerBase
    {
        private readonly ITaskManagementService _taskManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskManagmentController> _logger;

        public TaskManagmentController(ITaskManagementService taskManagementService, IMapper mapper, ILogger<TaskManagmentController> logger)
        {
            _taskManagementService = taskManagementService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetTask(string taskId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var jwt = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (jwt == null) return Unauthorized();
                if (AuthHelper.GetTokenClaims(jwt) == null) return Unauthorized();
                return Ok(await _taskManagementService.GetTask(taskId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetTask. Timestamp: {Timestamp}", DateTime.UtcNow);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var jwt = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (jwt == null) return Unauthorized();
                if (AuthHelper.GetTokenClaims(jwt) == null) return Unauthorized();
                var tasks = await _taskManagementService.GetAllsTasks();
                return Ok(_mapper.Map<List<GetTaskResponse>>(tasks)); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllTasks. Timestamp: {Timestamp}", DateTime.UtcNow);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTask([FromBody] AddTaskRequest _request)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var jwt = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (jwt == null) return Unauthorized();
                var claims = AuthHelper.GetTokenClaims(jwt);
                if (claims == null) return Unauthorized();
                if(string.IsNullOrEmpty(_request.username))
                    _request.username = claims.Username;
                return Ok(await _taskManagementService.UpdateTask(_mapper.Map<Entities.Tasks>(_request)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateTask. Timestamp: {Timestamp}", DateTime.UtcNow);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveTask([FromBody] RemoveTaskRequest _request)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var jwt = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (jwt == null) return Unauthorized();
                if (AuthHelper.GetTokenClaims(jwt) == null) return Unauthorized();
                return Ok(await _taskManagementService.RemoveTask(_request.taskId.ToString()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in RemoveTask. Timestamp: {Timestamp}", DateTime.UtcNow);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}