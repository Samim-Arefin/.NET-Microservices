using MediatR;

namespace Ordering.Application.Notifications
{
    public class OrderGeneratedNotification : INotification
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public OrderGeneratedNotification(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}
