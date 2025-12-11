using Ganss.Xss;

namespace CommentsSpaApi.Services.Html
{
    public class HtmlSanitizerService : IHtmlSanitizerService
    {
        private readonly HtmlSanitizer _sanitizer;

        public HtmlSanitizerService()
        {
            _sanitizer = new HtmlSanitizer();

            _sanitizer.AllowedTags.Clear();
            _sanitizer.AllowedTags.Add("a");
            _sanitizer.AllowedTags.Add("code");
            _sanitizer.AllowedTags.Add("i");
            _sanitizer.AllowedTags.Add("strong");

            _sanitizer.AllowedAttributes.Clear();
            _sanitizer.AllowedAttributes.Add("href");
            _sanitizer.AllowedAttributes.Add("title");

            _sanitizer.AllowedSchemes.Add("http");
            _sanitizer.AllowedSchemes.Add("https");
        }

        public string Sanitize(string html)
        {
            return _sanitizer.Sanitize(html ?? string.Empty);
        }
    }
}
