namespace Cart.API.Infrastructure
{
    public static class AppSettings
    {
        public static Settings Settings { get; set; } = new();
        public static GrpcSettings GrpcSettings { get; set; } = new();
        public static MessageBrokerSettings MessageBrokerSettings { get; set; } = new();
    }

    public class Settings
    {
        public string ConnectionStrings { get; set; }
    }

    public class GrpcSettings
    {
        public string DiscountGrpcUri { get; set; }
    }

    public class MessageBrokerSettings
    {
        public string Host { get; set; }
    }
}
