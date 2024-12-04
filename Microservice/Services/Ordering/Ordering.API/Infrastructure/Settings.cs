namespace Ordering.API.Infrastructure
{
    public static class Settings
    {
        public static MessageBrokerSettings MessageBrokerSettings { get; set; } = new();
    }

    public class MessageBrokerSettings
    {
        public string Host { get; set; }
    }
}
