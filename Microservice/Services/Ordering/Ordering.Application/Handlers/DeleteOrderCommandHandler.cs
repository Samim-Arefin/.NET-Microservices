using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Common.Persistence;
using Ordering.Application.Responses;
using System.Net;

namespace Ordering.Application.Handlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, UnitResponse>
    {
        private readonly IOrderRepository _orderRepository;
       
        public DeleteOrderCommandHandler(IOrderRepository orderRepository, IPublisher publisher) 
            => _orderRepository = orderRepository;

        public async Task<UnitResponse> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            await _orderRepository.DeleteOrder(request.Id);
            return new(statusCode: (int)HttpStatusCode.OK, isSuccess: true, message: "Successfully deleted!");
        }
    }
}
