using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Interfaces.Repositories;
using CManager.Web.DbContext;
using CManager.Core.Interfaces.Repositories;
using CManager.Data.Repositories.Base;

namespace CManager.Data.Repositories
{
    public class AttachmentRepository : BaseRepository<Attachments>, IAttachmentRepository
    {
        private CManagerDbContext _context;
        public AttachmentRepository(CManagerDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<Attachments>> GetAttachmentsByCourseId(int id)
        {
            var result = _context.Set<Attachments>().Where(x => x.Id == id);
            return await result.ToListAsync();//await All(x => x.CourseId == id).ToListAsync();
        }

        public Task Remove(int attachmentId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAttachments(Attachments attachment)
        {
            throw new NotImplementedException();
        }
    }
}
