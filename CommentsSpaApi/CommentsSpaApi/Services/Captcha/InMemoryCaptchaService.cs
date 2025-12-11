
using Microsoft.Extensions.Caching.Memory;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace CommentsSpaApi.Services.Captcha
{
    public class InMemoryCaptchaService : ICaptchaService
    {
        private readonly IMemoryCache _cache;
        private readonly Random _random = new Random();

        private const int CaptchaLength = 5;
        private const string CaptchaChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        private static readonly TimeSpan CaptchaLifetime = TimeSpan.FromMinutes(2);

        public InMemoryCaptchaService(IMemoryCache cache)
        {
            _cache = cache;
        }


        public (string captchaId, string answer, byte[] imageBytes, DateTime expiresAtUtc) GenerateCaptcha()
        {
            string answer = GenerateRandomText(CaptchaLength);
            string id = Guid.NewGuid().ToString("N");
            DateTime expireAtUtc = DateTime.UtcNow.Add(CaptchaLifetime);

            byte[] imageBytes = GenerateImage(answer);

            _cache.Set(id, answer, CaptchaLifetime);            

            return (id, answer, imageBytes, expireAtUtc);
        }

        public bool Validate(string captchaId, string input)
        {
            if (string.IsNullOrWhiteSpace(captchaId) || string.IsNullOrWhiteSpace(input))
            {                
                return false;
            }

            if (_cache.TryGetValue<string>(captchaId, out var expected))
            {
                var normalizedExpected = expected.Trim();
                var normalizedInput = input.Trim();

                var ok = string.Equals(normalizedExpected, normalizedInput, StringComparison.OrdinalIgnoreCase);                

                if (ok)
                {
                    _cache.Remove(captchaId);
                }

                return ok;
            }
            
            return false;
        }



        private string GenerateRandomText(int length)
        {
            var result = new char[length];
            
            for (int i = 0; i < length; i++)
            {
                int index = _random.Next(CaptchaChars.Length);
                result[i] = CaptchaChars[index];
            }
            
            return new string(result);
        }

        private byte[] GenerateImage(string text)
        {
            const int width = 150;
            const int height = 50;

            using var image = new Image<Rgba32>(width, height);

            var font = SystemFonts.CreateFont("DejaVu Sans", 24, FontStyle.Bold);

            image.Mutate(ctx =>
            {
                ctx.Fill(Color.White);
                ctx.DrawText(text, font, Color.Black, new PointF(10, 10));
            });

            using var ms = new MemoryStream();
            image.SaveAsPng(ms);
            return ms.ToArray();
        }
    }
}
