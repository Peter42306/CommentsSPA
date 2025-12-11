using CommentsSpaApi.Domain.Entities;

namespace CommentsSpaApi.Services.Attachments
{
    public interface IAttachmentService
    {
        Task<Attachment> UploadAsync(int commentId, IFormFile file);
        Task<List<Attachment>> GetForCommentAsync(int commentId);
        Task<(Attachment attachment, string fullPath)?> GetFileAsync(int attachmentId);
    }
}
