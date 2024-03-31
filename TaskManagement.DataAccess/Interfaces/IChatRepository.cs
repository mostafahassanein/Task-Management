﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Entities;

namespace TaskManagement.DataAccess.Interfaces
{
    public interface IChatRepository
    {
        Task<bool> AddMessage(ChatMessage chatMessage);
        Task<IEnumerable<ChatMessage>> GetChatHistory();
    }
}
