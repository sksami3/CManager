using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;
using Unkowledge.Core.Interfaces.Repositories.Base;

namespace UKnowledge.Core.Interfaces.Repositories
{
    public interface IMessageRepositoy : IBaseRepository<Message>
    {
        Task Remove(int messageId);
        Task UpdateMessage(Message message);
        Task<List<Message>> GetMessageHistoryByRoleName(string roleName);
    }
}
