using System.Net;

namespace TodoAppLib.Middleware.StatusCodeExceptionMiddleware;

public class StatusCodeException : Exception
{
	public int StatusCode { get; init; }

    public StatusCodeException(HttpStatusCode code, string message)
        : base(message)
	{
        StatusCode = (int)code;
    }
}
