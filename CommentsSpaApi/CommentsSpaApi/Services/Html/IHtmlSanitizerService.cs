namespace CommentsSpaApi.Services.Html
{
    public interface IHtmlSanitizerService
    {
        string Sanitize(string html);
    }
}
