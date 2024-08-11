namespace basic_article_api.ApplicationExceptions
{
    public class NotFoundException(string message) : ApplicationException(message)
    {
    }
}
