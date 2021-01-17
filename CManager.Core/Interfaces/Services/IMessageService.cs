using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CManager.Core.Entity;

namespace CManager.Core.Interfaces.Services
{
    public interface IMessageService
    {
        Task Add(Message message);
        Task<IEnumerable<Message>> GetMessage();
        Task Update(Message message);
        Task Remove(int messageId);
        Task<Message> GetMessageById(int id);
        Task<List<Message>> GetMessageHistoryByRoleName(string roleName);
    }
}
