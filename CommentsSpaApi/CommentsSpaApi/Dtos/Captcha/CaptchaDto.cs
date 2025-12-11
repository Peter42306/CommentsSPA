namespace CommentsSpaApi.Dtos.Captcha
{
    public class CaptchaDto
    {    
        public string CaptchaId { get; set; } = null!;
        public string ImageBase64 { get; set; } = null!;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
