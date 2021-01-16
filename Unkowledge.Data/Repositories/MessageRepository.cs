using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;
using UKnowledge.Core.Interfaces.Repositories;
using UKnowledge.Web.DbContext;
using Unkowledge.Data.Repositories.Base;

namespace Uknowledge.Data.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepositoy
    {
        private UKnowledgeDbContext _context;
        public MessageRepository(UKnowledgeDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetMessageHistoryByRoleName(string roleName)
        {
            return await _context.Set<Message>().Where(m => m.ToRole.Name == roleName)
                    .Include(m => m.FromUser)
                    .Include(m => m.ToRole)
                    .OrderByDescending(m => m.CreatedDate)
                    .Reverse().ToListAsync();
        }

        public Task Remove(int messageId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
