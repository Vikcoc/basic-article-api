
using basic_article_api.ApplicationExceptions;

namespace basic_article_api.ArticleHandling.ArticleData
{
    public class ArticleRepo(ArticleDbSet dbSet) : IArticleRepo
    {
        private readonly ArticleDbSet _dbSet = dbSet;

        public void DeleteArticle(uint articleId)
        {
            _dbSet.RemoveArticle(articleId);
        }

        public ArticleDto GetArticle(uint articleId)
        {
            var art = _dbSet.Entities.FirstOrDefault(x => x.Id == articleId) ?? throw new NotFoundException("No article found");

            return new ArticleDto
            {
                Id = art.Id,
                Title = art.Title,
                Content = art.Content,
                PublishedDate = DateTime.Now,
            };
        }

        public List<ArticleDto> GetArticles(uint skip, uint onPage)
        {
            return _dbSet.Entities.OrderBy(x => x.Id)
                .Skip((int)skip)
                .Take((int)onPage)
                .Select(art => new ArticleDto
                {
                    Id = art.Id,
                    Title = art.Title,
                    Content = art.Content,
                    PublishedDate = DateTime.Now,
                })
                .ToList();
        }

        public void UpdateArticle(uint articleId, ArticleCreateDto article)
        {
            var art = _dbSet.Entities.FirstOrDefault(x => x.Id == articleId) ?? throw new NotFoundException("No article found");
            art.Title = article.Title;
            art.Content = article.Content;
            //await SaveChangesAsync();
        }

        public void CreateArticle(ArticleCreateDto newArticle)
        {
            // i trust higher validation
            _dbSet.AddArticle(new ArticleModel
            {
                Title = newArticle.Title,
                Content = newArticle.Content,
                PublishedDate = DateTimeOffset.UtcNow
            });
        }
    }
}
