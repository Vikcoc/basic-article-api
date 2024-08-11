namespace basic_article_api.ArticleApplicationAuth
{
    public class RolesRepoShort
    {
        private readonly Dictionary<string, string[]> emails = new Dictionary<string, string[]>
        {
            {"admin1", [ArticleApplicationAuthConstants.PermissionSetAdmin, ArticleApplicationAuthConstants.PermissionSetReader] },
            {"admin2", [ArticleApplicationAuthConstants.PermissionSetAdmin] },
        };

        public string[] GetRoles(string email)
        {
            return emails.ContainsKey(email) ? emails[email] : [ArticleApplicationAuthConstants.PermissionSetReader];
        }
    }
}
