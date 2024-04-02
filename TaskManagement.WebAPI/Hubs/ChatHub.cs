using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.WebAPI.Controllers;
using TaskManagement.WebAPI.Helper;

namespace TaskManagement.WebAPI.Hubs
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly ILogger<TaskManagmentController> _logger;
        private readonly IChatHistoryService _chatHistoryService;
        public ChatHub(ILogger<TaskManagmentController> logger, IChatHistoryService chatHistoryService)
        {
            _logger = logger;
            _chatHistoryService = chatHistoryService;
        }
        public async Task SendMessage(string sender, string message, string recipient)
        {
            try
            {
                await Clients.Group(recipient).SendAsync("ReceiveMessage", sender, message);
                await _chatHistoryService.AddMessage(new Entities.ChatMessage() { Message= message, Recipient=recipient, User=sender });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in OnConnectedAsync. Timestamp: {Timestamp}", DateTime.UtcNow);
            }
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                //var accessToken = Context.GetHttpContext().Request.Headers["Authorization"];
                var accessToken = Context.GetHttpContext().Request.Query["token"];
                string username = AuthHelper.GetTokenClaims(accessToken).Username;
                if (!string.IsNullOrEmpty(username))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, username);
                }

                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in OnConnectedAsync. Timestamp: {Timestamp}", DateTime.UtcNow);
            }
        }
    }
}
