using Flurl.Http.Configuration;

namespace ChatApplication.Blazor.Polly;

public class PollyHttpClientFactory: DefaultHttpClientFactory
{
    public override HttpMessageHandler CreateMessageHandler()
    {
        return new PolicyHandler
        {
            InnerHandler = base.CreateMessageHandler()
        };
    }
}