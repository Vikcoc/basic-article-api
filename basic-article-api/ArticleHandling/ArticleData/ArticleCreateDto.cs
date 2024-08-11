using System.ComponentModel.DataAnnotations;

namespace basic_article_api.ArticleHandling.ArticleData
{
    public struct ArticleCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
