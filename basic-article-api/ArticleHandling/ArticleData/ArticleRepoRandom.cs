
using System;

namespace basic_article_api.ArticleHandling.ArticleData
{
    public class ArticleRepoRandom : IArticleRepo
    {
        private readonly Random _random = new Random();
        public void CreateArticle(ArticleCreateDto newArticle)
        {}

        public void DeleteArticle(uint articleId)
        {}

        public ArticleDto GetArticle(uint articleId)
        {
            return new ArticleDto()
            {
                Id = articleId,
                PublishedDate = DateTime.UtcNow,
                Title = Enumerable.Repeat(RandomString(_random.Next(5, 10)), _random.Next(1, 3)).Aggregate((a, b) => a + " " + b),
                Content = Enumerable.Repeat(RandomString(_random.Next(5, 15)), _random.Next(1, 6)).Aggregate((a, b) => a + " " + b)
            };
        }

        public List<ArticleDto> GetArticles(uint skip, uint onPage)
        {
            var res = new List<ArticleDto>((int)onPage);
            for (uint i = 0; i < onPage; i++)
            {
                res.Add(new ArticleDto()
                {
                    Id = skip + i,
                    PublishedDate = DateTime.UtcNow,
                    Title = Enumerable.Repeat(RandomString(_random.Next(5, 10)), _random.Next(1, 3)).Aggregate((a, b) => a + " " + b),
                    Content = Enumerable.Repeat(RandomString(_random.Next(5, 15)), _random.Next(1, 6)).Aggregate((a, b) => a + " " + b)
                });
            }
            return res;
        }

        public void UpdateArticle(uint articleId, ArticleCreateDto article)
        {
            
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
