namespace basic_article_api.ApplicationExceptions
{
    public class ServiceUnavailableException(string message) : ApplicationException(message)
    {
    }
}
