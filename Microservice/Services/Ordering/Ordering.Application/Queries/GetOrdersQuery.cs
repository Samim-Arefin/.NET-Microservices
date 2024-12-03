using MediatR;
using Ordering.Application.Responses;


namespace Ordering.Application.Queries
{
    public class GetOrdersQuery : IRequest<List<OrderResponse>>
    {
        public string UserName { get; set; }
        public GetOrdersQuery(string userName)
            => UserName = userName;


    }
}
