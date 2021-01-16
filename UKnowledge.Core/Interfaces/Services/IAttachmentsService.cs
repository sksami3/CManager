using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;

namespace UKnowledge.Core.Interfaces.Services
{
    public interface IAttachmentsService
    {
        Task Add(Attachments attachments);
        Task<IEnumerable<Attachments>> GetAttachments();
        Task Update(Attachments attachments);
        Task Remove(int attachmentsId);
        Task<Attachments> GetAttachmentsById(int id);
        Task<List<Attachments>> GetAttachmentsByCourseId(int id);
    }
}
