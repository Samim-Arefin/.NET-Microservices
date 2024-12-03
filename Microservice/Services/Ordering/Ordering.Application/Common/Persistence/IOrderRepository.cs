using Ordering.Domain.Entities;

namespace Ordering.Application.Common.Persistence
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersByUserName(string userName);
        Task<Order> AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(Guid id);
    }
}
