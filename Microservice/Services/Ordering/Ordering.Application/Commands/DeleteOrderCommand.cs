using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Commands
{
    public class DeleteOrderCommand : IRequest<UnitResponse>
    {
        public Guid Id { get; set; }

        public DeleteOrderCommand(Guid id) 
            => Id = id;
    }
}
