namespace CommentsSpaApi.Dtos.Comments
{
    public class CommentListItemDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? HomePage { get; set; }

        public string SanitizedText { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; }
    }
}
