namespace basic_article_api.ArticleHandling.ArticleData
{
    public class ArticleModel
    {
        public uint Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTimeOffset PublishedDate { get; set; }
    }
}
