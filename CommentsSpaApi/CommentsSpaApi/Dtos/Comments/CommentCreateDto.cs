using System.ComponentModel.DataAnnotations;

namespace CommentsSpaApi.Dtos.Comments
{
    public class CommentCreateDto
    {
        public int? ParentId { get; set; }

        //[Required(ErrorMessage = "Name is required")]
        //[MaxLength(50)]
        //[RegularExpression("^[A-Za-z0-9]+$")]
        public string UserName { get; set; } = null!;

        //[Required(ErrorMessage = "Email is required")]
        //[MaxLength(100)]
        //[EmailAddress]
        public string Email { get; set; } = null!;

        //[MaxLength(200)]
        //[Url]
        public string? HomePage {  get; set; }

        //[Required]
        //[MaxLength(4000)]
        public string RawText { get; set; } = null!;

        //[Required]
        public string CaptchaId { get; set; } = null!;

        //[Required]
        public string CaptchaInput { get; set; } = null!;
    }
}
