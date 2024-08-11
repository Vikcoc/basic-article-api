namespace basic_article_api.ApplicationExceptions
{
    public class BadRequestException(string message) : ApplicationException(message)
    {
    }
}
