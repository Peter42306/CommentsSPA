using CommentsSpaApi.Dtos.Captcha;
using CommentsSpaApi.Services.Captcha;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommentsSpaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private readonly ICaptchaService _captchaService;

        public CaptchaController(ICaptchaService captchaService)
        {
            _captchaService = captchaService;
        }

        // GET: api/captcha
        [HttpGet]
        public ActionResult<CaptchaDto> GetNew()
        {
            var tuple = _captchaService.GenerateCaptcha();

            var captchaId = tuple.captchaId;
            var answer = tuple.answer;
            var imageBytes = tuple.imageBytes;
            var expireAtUtc = tuple.expiresAtUtc;


            var dto = new CaptchaDto
            {
                CaptchaId = captchaId,
                ImageBase64 = "data:image/png;base64," + Convert.ToBase64String(imageBytes),
                ExpiresAtUtc = expireAtUtc
            };

            return Ok(dto);
        }
    }
}
