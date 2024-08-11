using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace basic_article_api.ArticleApplicationAuth.Services
{
    public class ArticleApplicationJwtProvider(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public string MakeAccessToken(string email, string[] roles)
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(_configuration["SigningCredentials:Auth:PrivateRSAKey"]);
            var securitykey = new RsaSecurityKey(rsa)
            {
                KeyId = _configuration["SigningCredentials:JWK:kid"]
            };

            var desc = new SecurityTokenDescriptor()
            {
                Expires = DateTimeOffset.Now.AddMinutes(int.Parse(_configuration["SigningCredentials:Auth:ExpiresAddMinutes"]!)).DateTime,
                SigningCredentials = new SigningCredentials(securitykey, _configuration["SigningCredentials:Auth:JWK:alg"]),
                Audience = _configuration["SigningCredentials:Auth:Audience"],
                Issuer = _configuration["SigningCredentials:Auth:Issuer"],
                NotBefore = DateTimeOffset.Now.DateTime,
                IssuedAt = DateTimeOffset.Now.DateTime,
                TokenType = "JWT",
                Claims = new Dictionary<string, object>() {
                    { ArticleApplicationAuthConstants.EmailParam, email },
                    { ArticleApplicationAuthConstants.PermissionSet, roles }
                }
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.CreateJwtSecurityToken(desc).RawData;
        }
    }
}
