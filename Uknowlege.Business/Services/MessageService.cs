using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;
using UKnowledge.Core.Interfaces.Repositories;
using UKnowledge.Core.Interfaces.Services;

namespace Uknowledge.Business.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepositoy _messageRepository;

        public MessageService(IMessageRepositoy messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task Add(Message message)
        {
            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetMessage()
        {
            return await _messageRepository.All().ToListAsync();
        }

        public async Task<Message> GetMessageById(int id)
        {
            return await _messageRepository.FindAsync(id);
        }

        public async Task<List<Message>> GetMessageHistoryByRoleName(string roleName)
        {
            return await _messageRepository.GetMessageHistoryByRoleName(roleName);
        }

        public async Task Remove(int messageId)
        {
            await _messageRepository.Remove(messageId);
        }

        public async Task Update(Message message)
        {
            await _messageRepository.UpdateMessage(message);
        }
    }
}
