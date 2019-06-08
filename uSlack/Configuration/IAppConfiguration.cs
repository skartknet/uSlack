namespace uSlack.Configuration
{
    public interface IAppConfiguration
    {
        MessagesConfiguration Messages { get; }
        string Token { get; }
        string SlackChannel { get; }
    }
}