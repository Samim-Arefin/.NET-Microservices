using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ISender _sender;
        public OrderController(ISender sender)
            => _sender = sender;

        [HttpGet, Route("{userName}")]
        [ResponseCache(Duration = 10)]
        public async Task<IActionResult> GetOrders(string userName)
            => Ok(await _sender.Send(new GetOrdersQuery(userName)));

        [HttpPost]
        public async Task<IActionResult> CheckoutOrder(CheckoutOrderCommand command)
            => Ok(await _sender.Send(command));

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderCommand command)
            => Ok(await _sender.Send(command));

        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
            => Ok(await _sender.Send(new DeleteOrderCommand(id)));
    }
}
