using CommentsSpaApi.Domain.Enums;

namespace CommentsSpaApi.Dtos.Attachments
{
    public class AttachmentDto
    {
        public int Id { get; set; }

        public string OriginalFileName { get; set; } = null!;
        public string Url { get; set; } = null!;

        public AttachmentType Type { get; set; }

        public long SizeBytes { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
