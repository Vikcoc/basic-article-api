using basic_article_api.ApplicationExceptions;
using Microsoft.IdentityModel.Tokens;

namespace basic_article_api.ArticleApplicationAuth.Services
{
    public class GoogleJwtValidator(GenericJwtValidator genericValidator, IConfiguration configuration, HttpClient httpClient) : IJwtValidator
    {
        private readonly GenericJwtValidator _genericValidator = genericValidator;
        private readonly IConfiguration _configuration = configuration;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<string> ValidateAndExtractEmailAsync(string token)
        {
            #region Get Validation Params
            var keysResponse = await _httpClient.GetAsync(_configuration["GoogleToken:KeysUrl"]);

            if (keysResponse == null || !keysResponse.IsSuccessStatusCode)
                throw new ServiceUnavailableException("Cannot get Google signing keys");

            var keysString = await keysResponse.Content.ReadAsStringAsync();
            var keySet = new JsonWebKeySet(keysString);

            var tPar = new TokenValidationParameters()
            {
                ValidIssuer = _configuration["GoogleToken:Issuer"],
                ValidAudience = _configuration["GoogleToken:Audience"],
                IssuerSigningKeys = keySet.Keys,
            };
            #endregion

            var validToken = _genericValidator.GetValidToken(token, tPar);

            var props = validToken.Claims.ToDictionary(x => x.Type, x => x.Value);

            //I trust that the google token will always have email
            return props["email"];
        }
    }
}
