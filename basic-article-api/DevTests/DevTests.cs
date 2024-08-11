using basic_article_api.ApplicationExceptions;
using basic_article_api.ArticleApplicationAuth;

namespace basic_article_api.DevTests
{
    public static class DevTests
    {
        public static IEndpointRouteBuilder AddTestRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/hello", () =>
            {
                return TypedResults.Ok("Hello World!");
            }).RequireAuthorization(p => p.RequireClaim(ArticleApplicationAuthConstants.PermissionSet, ArticleApplicationAuthConstants.PermissionSetReader));

            endpoints.MapGet("throw400", () => {
                throw new BadRequestException("Neet to use");
                });

            endpoints.MapGet("throw500", () => {
                throw new Exception("Big Problem");
            });

            endpoints.MapGet("throw503", () => {
                throw new ServiceUnavailableException("External Problem");
            });

            return endpoints;
        }
    }
}
