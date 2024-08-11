namespace basic_article_api.ArticleHandling.ArticleData
{
    public interface IArticleRepo
    {
        public List<ArticleDto> GetArticles(uint skip, uint onPage);
        public ArticleDto GetArticle(uint articleId);
        public void CreateArticle(ArticleCreateDto newArticle);
        public void UpdateArticle(uint articleId, ArticleCreateDto article);
        public void DeleteArticle(uint articleId);
    }
}
