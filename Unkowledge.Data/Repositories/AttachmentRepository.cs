using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;
using UKnowledge.Core.Interfaces.Repositories;
using UKnowledge.Web.DbContext;
using Unkowledge.Core.Interfaces.Repositories;
using Unkowledge.Data.Repositories.Base;

namespace Unkowledge.Data.Repositories
{
    public class AttachmentRepository : BaseRepository<Attachments>, IAttachmentRepository
    {
        private UKnowledgeDbContext _context;
        public AttachmentRepository(UKnowledgeDbContext context)
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
