namespace CommentsSpaApi.Services.Captcha
{
    public interface ICaptchaService
    {
        (string captchaId, string answer, byte[] imageBytes, DateTime expiresAtUtc) GenerateCaptcha();
        bool Validate(string captchaId, string input);
    }
}
