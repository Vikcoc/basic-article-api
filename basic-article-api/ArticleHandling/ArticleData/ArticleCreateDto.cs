using System.ComponentModel.DataAnnotations;

namespace basic_article_api.ArticleHandling.ArticleData
{
    public struct ArticleCreateDto
    {
        [MaxLength(5)]
        public string Title { get; set; }
        [MaxLength(10)]
        public string Content { get; set; }
    }
}
