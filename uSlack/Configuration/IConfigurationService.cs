namespace uSlack.Configuration
{
    public interface IConfigurationService
    {
        MessagesConfiguration MessagesConfiguration { get; }
        UslackConfiguration AppConfiguration { get; }

        void Initialize();
    }
}