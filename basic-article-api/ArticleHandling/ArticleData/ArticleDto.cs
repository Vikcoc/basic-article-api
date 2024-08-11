namespace basic_article_api.ArticleHandling.ArticleData
{
    public struct ArticleDto
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset PublishedDate { get; set; }
    }
}
