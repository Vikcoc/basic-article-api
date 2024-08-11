using basic_article_api.ArticleApplicationAuth.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace basic_article_api.ArticleApplicationAuth
{
    public static class ArticleAuthController
    {
        public static IEndpointRouteBuilder AddAuthRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("api/auth/google", ProcessGoogleTokenAsync); // could've used get and put the token in the route, but nah

            return endpoints;
        }
        public static IEndpointRouteBuilder AddDevAuth(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("api/auth/dev/{email}", DevLocalToken); // won't use a different approach for routes because i don't want to get required service config, get root route for api
            //and because a reverse proxy can do that for me

            return endpoints;
        }

        public static void AddAuthServices(this IServiceCollection services)
        {
            services.AddScoped<JwtSecurityTokenHandler>();
            services.AddScoped<GenericJwtValidator>();
            services.AddKeyedScoped<IJwtValidator, GoogleJwtValidator>("Google"); // keyed because maybe we want oauth with Microsoft, and the only difference should be how we get the keyset and maybe how the email is stored
            services.AddScoped<ArticleApplicationJwtProvider>();
            services.AddSingleton<RolesRepoShort>(); // based on the EF core convention, should be scoped, but this is also the database and is readonly
        }

        public static async Task<string> ProcessGoogleTokenAsync([FromBody] string token, [FromKeyedServices("Google")] IJwtValidator validator, [FromServices] RolesRepoShort rolesRepo, [FromServices] ArticleApplicationJwtProvider tokenProvider)
            => await ComputeAccessTokenAsync(token, validator, rolesRepo, tokenProvider);

        public static string DevLocalToken([FromRoute] string email, [FromServices] RolesRepoShort rolesRepo, [FromServices] ArticleApplicationJwtProvider tokenProvider)
        {
            var roles = rolesRepo.GetRoles(email);
            return tokenProvider.MakeAccessToken(email, roles);
        }

        private static async Task<string> ComputeAccessTokenAsync(string token, IJwtValidator validator, RolesRepoShort rolesRepo, ArticleApplicationJwtProvider tokenProvider)
        {
            var email = await validator.ValidateAndExtractEmailAsync(token);
            var roles = rolesRepo.GetRoles(email);
            return tokenProvider.MakeAccessToken(email, roles);
        }
    }
}
