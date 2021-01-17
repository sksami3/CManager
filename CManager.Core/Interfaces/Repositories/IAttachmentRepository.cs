using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CManager.Core.Entity;
using CManager.Core.Interfaces.Repositories.Base;

namespace CManager.Core.Interfaces.Repositories
{
    public interface IAttachmentRepository : IBaseRepository<Attachments>
    {
        Task Remove(int attachmentId);
        Task UpdateAttachments(Attachments attachment);
        Task<List<Attachments>> GetAttachmentsByCourseId(int id);
    }
}
