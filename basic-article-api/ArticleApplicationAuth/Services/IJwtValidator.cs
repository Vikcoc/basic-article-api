namespace basic_article_api.ArticleApplicationAuth.Services
{
    public interface IJwtValidator
    {
        public Task<string> ValidateAndExtractEmailAsync(string token);
    }
}
