namespace Ordering.Infrastructure.Extensions
{
    public static class AppSettings
    {
        public static Settings Settings { get; set; } = new();
        public static EmailSettings EmailSettings { get; set; } = new();
    }

    public class Settings
    {
        public string ConnectionStrings { get; set; }
    }

    public class EmailSettings
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string EmailFrom { get; set; }
        public string MailPassword { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
