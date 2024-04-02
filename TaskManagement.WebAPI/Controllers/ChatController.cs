using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using TaskManagement.Application.Interfaces;
using TaskManagement.Entities;
using TaskManagement.WebAPI.Helper;
using TaskManagement.WebAPI.Hubs;
using TaskManagement.WebAPI.Models.DTOs;
using TaskManagement.WebAPI.Models.Requests;
using TaskManagement.WebAPI.Models.Responses;

namespace TaskManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatHistoryService _chatHistoryService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<ChatController> _logger;
        private readonly List<string> friends = new List<string>() { "mostafa", "adham", "lara" };
        public ChatController(IMapper mapper, IChatHistoryService chatHistoryService, IHubContext<ChatHub> hubContext, ILogger<ChatController> logger)
        {
            _mapper = mapper;
            _hubContext = hubContext;
            _chatHistoryService = chatHistoryService;
            _logger = logger;
        }
        [HttpPost("GetChatHistory")]
        public async Task<ActionResult<List<GetChatHistoryResponse>>> GetChatHistory([FromBody] GetChatHistoryRequest _request)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var jwt = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (jwt == null) return Unauthorized();
                var claims = AuthHelper.GetTokenClaims(jwt);
                if (claims == null) return Unauthorized();
                List<ChatMessage> chatHistory = _chatHistoryService.GetChatHistory().Result
                    .Where(msg => (msg.User == claims.Username && msg.Recipient == _request.recipient) || (msg.User == _request.recipient && msg.Recipient == claims.Username))
                    .ToList();
               return Ok(_mapper.Map<List<GetChatHistoryResponse>>(chatHistory));
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetChatHistory. Timestamp: {Timestamp}", DateTime.UtcNow);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("SendMessage")]
        public async Task<ActionResult> SendMessage([FromBody] ChatMessageDTO _request)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var jwt = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (jwt == null) return Unauthorized();
                if (AuthHelper.GetTokenClaims(jwt) == null) return Unauthorized();
                await _chatHistoryService.AddMessage(_mapper.Map<Entities.ChatMessage>(_request));
                await _hubContext.Clients.Group(_request.Recipient).SendAsync("ReceiveMessage", _request.User, _request.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in SendMessage. Timestamp: {Timestamp}", DateTime.UtcNow);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetFriendsList")]
        public async Task<ActionResult<IEnumerable<UserInfoResponse>>> GetFriendsList()
        {
            try
            {

                if (!ModelState.IsValid) return BadRequest(ModelState);
                var jwt = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (jwt == null) return Unauthorized();
                var claims = AuthHelper.GetTokenClaims(jwt);
                if (claims == null) return Unauthorized();
                List<string> updatedFriends = friends.Where(f => f != claims.Username).ToList();
                List<UserInfoResponse> list = new List<UserInfoResponse>();
                foreach (var item in updatedFriends)
                {
                    list.Add(new UserInfoResponse() { username = item });
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetChatHistory. Timestamp: {Timestamp}", DateTime.UtcNow);
                return StatusCode(500, "Internal server error");
            }
        }
        
    }
}
