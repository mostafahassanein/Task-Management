using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.DataAccess.Interfaces;
using TaskManagement.Entities;

namespace TaskManagement.Application.Implementations
{
    internal class ChatHistoryService : IChatHistoryService
    {
        private readonly IChatRepository _chatRepository;
        //private readonly List<ChatMessage> _chatHistory;

        public ChatHistoryService(IChatRepository chatRepository)
        {
            _chatRepository= chatRepository;
            //_chatHistory = new List<ChatMessage>();
        }
        public async Task AddMessage(ChatMessage chatMessage)
        {
            //_chatHistory.Add(chatMessage);
            await _chatRepository.AddMessage(chatMessage);
        }

        public async Task<IEnumerable<ChatMessage>> GetChatHistory()
        {
            return await _chatRepository.GetChatHistory();
        }
    }
}
