using CommentsSpaApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CommentsSpaApi.Domain.Entities
{
    public class Attachment
    {
        public int Id { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; } = null!;

        [Required, MaxLength(260)]
        public string OriginalFileName { get; set; } = null!;

        [Required, MaxLength(260)]
        public string StoredFileName { get; set; } = null!;

        [Required, MaxLength(500)]
        public string RelativePath { get; set; } = null!;

        [Required]
        public AttachmentType Type { get; set; }

        public long SizeBytes { get; set; }

        [MaxLength(100)]
        public string? MimeType { get; set; }

        // for images
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
