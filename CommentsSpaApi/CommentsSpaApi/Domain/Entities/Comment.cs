using System.ComponentModel.DataAnnotations;

namespace CommentsSpaApi.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        // cascade of notes
        public int? ParentId { get; set; }
        public Comment? Parent { get; set; }
        public List<Comment> Replies { get; set; } = new List<Comment>();

        
        
        [Required, MaxLength(50)]
        [RegularExpression("^[A-Za-z0-9]+$")]
        public string UserName { get; set; } = null!;

        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [MaxLength(200)]
        [Url]
        public string? HomePage { get; set; }



        [Required, MaxLength(4000)]
        public string RawText { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string SanitizedText { get; set; } = null!;

        
        
        [MaxLength(50)]
        public string? UserIp { get; set; }

        [MaxLength(512)]
        public string? UserAgent { get; set; }

        
        
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
