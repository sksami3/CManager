using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Interfaces.Repositories;
using CManager.Web.DbContext;
using CManager.Data.Repositories.Base;

namespace CManager.Data.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepositoy
    {
        private CManagerDbContext _context;
        public MessageRepository(CManagerDbContext context)
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
