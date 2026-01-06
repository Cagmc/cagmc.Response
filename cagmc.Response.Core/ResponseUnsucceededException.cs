namespace cagmc.Response.Core;

public class ResponseUnsucceededException : Exception
{
    public ResponseUnsucceededException() : base()
    {
    }
    
    public ResponseUnsucceededException(string message) : base(message)
    {
    }
}