using basic_article_api.ApplicationExceptions;
using basic_article_api.ArticleApplicationAuth;
using basic_article_api.ArticleHandling.ArticleData;
using Microsoft.AspNetCore.Mvc;

namespace basic_article_api.ArticleHandling
{
    public static class ArticleHandlingController
    {
        public static IEndpointRouteBuilder AddArticleRoutes(this IEndpointRouteBuilder endpoints)
        {
            //with mininal apis validation seems different https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/min-api-filters?view=aspnetcore-8.0#validate-an-object-with-a-filter

            endpoints.MapGet("api/articles", GetArticles)
                .RequireAuthorization(p => p.RequireClaim(ArticleApplicationAuthConstants.PermissionSet, ArticleApplicationAuthConstants.PermissionSetReader));
            endpoints.MapGet("api/articles/{id}", GetArticle)
                .RequireAuthorization(p => p.RequireClaim(ArticleApplicationAuthConstants.PermissionSet, ArticleApplicationAuthConstants.PermissionSetReader));
            endpoints.MapPost("api/articles", CreateArticle)
                .AddEndpointFilter(async (efiContext, next) =>
                {
                    var tdparam = efiContext.GetArgument<ArticleCreateDto>(0);
                    ArbitraryValidationThrow(tdparam);
                    return await next(efiContext);
                })
                .RequireAuthorization(p => p.RequireClaim(ArticleApplicationAuthConstants.PermissionSet, ArticleApplicationAuthConstants.PermissionSetAdmin));
            endpoints.MapPut("api/articles/{id}", UpdateArticle)
                .AddEndpointFilter(async (efiContext, next) =>
                {
                    var tdparam = efiContext.GetArgument<ArticleCreateDto>(0);
                    ArbitraryValidationThrow(tdparam);
                    return await next(efiContext);
                })
                .RequireAuthorization(p => p.RequireClaim(ArticleApplicationAuthConstants.PermissionSet, ArticleApplicationAuthConstants.PermissionSetAdmin));
            endpoints.MapDelete("api/articles/{id}", DeleteArticle)
                .RequireAuthorization(p => p.RequireClaim(ArticleApplicationAuthConstants.PermissionSet, ArticleApplicationAuthConstants.PermissionSetAdmin));

            return endpoints;
        }

        public static void AddArticleServices(this IServiceCollection services)
        {
            services.AddSingleton<ArticleDbSet>(s =>
            {
                var seeder = s.GetRequiredKeyedService<ArticleRepoRandom>("Seeder");
                var artDb = new ArticleDbSet();
                foreach (var art in seeder.GetArticles(0, 50))
                    artDb.AddArticle(new ArticleModel
                    {
                        Title = art.Title,
                        Content = art.Content,
                        PublishedDate = art.PublishedDate
                    });
                return artDb;

            }); // a database kinda is global
            services.AddScoped<IArticleRepo, ArticleRepo>(); // scoped by the convention set by Ef core (because the tracker, though we don't have tracker here)
            //services.AddScoped<IArticleRepo, ArticleRepoRandom>(); for fun, and because i needed a reason to have an interface
            services.AddKeyedSingleton<ArticleRepoRandom>("Seeder");

        }

        public static List<ArticleDto> GetArticles([FromQuery] uint skip, [FromQuery] uint onPage, [FromServices] IArticleRepo repo)
        {
            if (onPage > 100)
                throw new BadRequestException("Too many items on page");
            return repo.GetArticles(skip, onPage);
        }
        public static ArticleDto GetArticle([FromRoute] uint id, [FromServices] IArticleRepo repo)
            => repo.GetArticle(id);
        public static void CreateArticle([FromBody] ArticleCreateDto article, [FromServices] IArticleRepo repo)
        {
            repo.CreateArticle(article);
        }

        public static void UpdateArticle([FromRoute] uint id, [FromBody] ArticleCreateDto article, [FromServices] IArticleRepo repo)
        {
            repo.UpdateArticle(id, article);
        }

        public static void DeleteArticle([FromRoute] uint id, [FromServices] IArticleRepo repo)
            => repo.DeleteArticle(id);


        private static void ArbitraryValidationThrow(ArticleCreateDto article)
        {
            // new validation dropped https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/min-api-filters?view=aspnetcore-8.0#validate-an-object-with-a-filter

            if (article.Title.Length > 100)
                throw new BadRequestException("Title is too long");
            if (article.Content.Length > 1000)
                throw new BadRequestException("Content is too long");
            if (article.Title.Contains("peanut") || article.Content.Contains("peanut"))
                throw new BadRequestException("Article cannot contain the word 'peanut'");
        }
    }
}
