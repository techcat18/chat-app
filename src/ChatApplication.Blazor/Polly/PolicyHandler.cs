namespace ChatApplication.Blazor.Polly;

public class PolicyHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Policies.ExponentialRetryPolicy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
    }
}