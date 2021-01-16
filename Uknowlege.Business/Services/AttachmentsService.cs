using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UKnowledge.Core.Entity;
using UKnowledge.Core.Interfaces.Repositories;
using UKnowledge.Core.Interfaces.Services;

namespace Uknowlege.Business.Services
{
    public class AttachmentsService : IAttachmentsService
    {
        private readonly IAttachmentRepository _attachmentRepository;

        public AttachmentsService(IAttachmentRepository attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public async Task Add(Attachments attachment)
        {
            await _attachmentRepository.AddAsync(attachment);
            await _attachmentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Attachments>> GetAttachments()
        {
            return await _attachmentRepository.All().ToListAsync();
        }

        public async Task<List<Attachments>> GetAttachmentsByCourseId(int id)
        {
            return await _attachmentRepository.GetAttachmentsByCourseId(id);
        }

        public async Task<Attachments> GetAttachmentsById(int id)
        {
            return await _attachmentRepository.FindAsync(id);
        }
        public async Task Remove(int attachmentId)
        {
            await _attachmentRepository.Remove(attachmentId);
        }

        public async Task Update(Attachments attachment)
        {
            await _attachmentRepository.UpdateAttachments(attachment);
        }
    }
}
