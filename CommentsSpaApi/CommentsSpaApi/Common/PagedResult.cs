namespace CommentsSpaApi.Common
{
    // helper for pagination
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();

        public int Page {  get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }

        public int TotalPages 
        { 
            get 
            {
                double total = TotalCount;
                double size = PageSize;
                double rawPages = total / size;
                double roundedPages = Math.Ceiling(rawPages);
                int result = (int)roundedPages;
                return result;
            } 
        }
    }
}
