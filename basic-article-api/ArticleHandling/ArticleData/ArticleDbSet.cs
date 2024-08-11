using basic_article_api.ApplicationExceptions;

namespace basic_article_api.ArticleHandling.ArticleData
{
    public class ArticleDbSet
    {
        private readonly List<ArticleModel> _arts = [];
        public IEnumerable<ArticleModel> Entities => _arts;

        private uint index = 0;

        public uint AddArticle(ArticleModel article)
        {
            lock (this)
            {
                article.Id = index;
                index++;
                _arts.Add(article);
            }
            return article.Id;
        }

        public void RemoveArticle(uint articleId)
        {
            var art = _arts.FirstOrDefault(x => x.Id == articleId) ?? throw new NotFoundException("No article found");
            // could be a 400?
            // could also be just a return
            _arts.Remove(art);
        }
    }
}
