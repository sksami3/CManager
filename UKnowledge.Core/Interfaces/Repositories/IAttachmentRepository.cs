using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;
using Unkowledge.Core.Interfaces.Repositories.Base;

namespace UKnowledge.Core.Interfaces.Repositories
{
    public interface IAttachmentRepository : IBaseRepository<Attachments>
    {
        Task Remove(int attachmentId);
        Task UpdateAttachments(Attachments attachment);
        Task<List<Attachments>> GetAttachmentsByCourseId(int id);
    }
}
