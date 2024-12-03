using MediatR;
using Ordering.Application.Common.Infrastructure;
using Ordering.Application.Notifications;

namespace Ordering.Application.Handlers
{
    public class EmailNotificationHandler : INotificationHandler<OrderGeneratedNotification>
    {
        private readonly IEmailService _emailService;
        public EmailNotificationHandler(IEmailService emailService) 
            => _emailService = emailService;
        
        public async Task Handle(OrderGeneratedNotification notification, CancellationToken cancellationToken)
        {
            await _emailService.SendEmailAsync(notification.To, notification.Subject, notification.Body);
        }
    }
}
