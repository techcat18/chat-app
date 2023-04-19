using System.Net;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace ChatApplication.Blazor.Polly;

public static class Policies
{
    private static bool IsTransientError(FlurlHttpException exception)
    {
        var httpStatusCodesWorthRetrying = new List<int>
        {
            (int)HttpStatusCode.RequestTimeout,
            (int)HttpStatusCode.BadGateway,
            (int)HttpStatusCode.ServiceUnavailable,
            (int)HttpStatusCode.GatewayTimeout 
        };

        return exception.StatusCode.HasValue && httpStatusCodesWorthRetrying.Contains(exception.StatusCode.Value);
    }
    
    public static AsyncRetryPolicy ExponentialRetryPolicy
    {
        get
        {
            var retryPolicy = Policy
                .Handle<FlurlHttpException>(IsTransientError)
                .WaitAndRetryAsync(3, retryAttempt =>
                {
                    var nextAttemptIn = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                    Console.WriteLine($"Retry attempt {retryAttempt} to make request. Next try on {nextAttemptIn.TotalSeconds} seconds.");
                    return nextAttemptIn;
                });
    
            return retryPolicy;
        }
    }
}