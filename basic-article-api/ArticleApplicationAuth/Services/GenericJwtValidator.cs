using basic_article_api.ApplicationExceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace basic_article_api.ArticleApplicationAuth.Services
{
    public class GenericJwtValidator(JwtSecurityTokenHandler tokenHandler)
    {
        private readonly JwtSecurityTokenHandler _tokenHandler = tokenHandler;

        public JwtSecurityToken GetValidToken(string token, TokenValidationParameters parameters)
        {
            // should assert parameters are not null
            // but I trust myself that they are not

            parameters.RequireExpirationTime = true;
            parameters.ClockSkew = TimeSpan.Zero;

            JwtSecurityToken parsedToken;
            try
            {
                _tokenHandler.ValidateToken(token, parameters, out var abstractToken);
                parsedToken = (JwtSecurityToken)abstractToken;
            }
            catch (Exception)
            {
                throw new BadRequestException("Invalid token"); // a nicer message could have happened
            }

            return parsedToken;
        }

    }
}
