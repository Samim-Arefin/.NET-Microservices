namespace Cart.API.Infrastructure
{
    public static class AppSettings
    {
        public static Settings Settings { get; set; } = new();
    }

    public class Settings
    {
        public string ConnectionStrings { get; set; }
    }
}
