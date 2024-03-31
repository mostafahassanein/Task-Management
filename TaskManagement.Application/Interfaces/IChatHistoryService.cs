using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Entities;

namespace TaskManagement.Application.Interfaces
{
    public interface IChatHistoryService
    {
        Task AddMessage(ChatMessage chatMessage);
        Task<IEnumerable<ChatMessage>> GetChatHistory();
    }
}
