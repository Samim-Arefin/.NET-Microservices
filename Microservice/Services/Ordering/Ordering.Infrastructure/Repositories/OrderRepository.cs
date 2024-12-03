using Dapper;
using Ordering.Application.Common.Persistence;
using Ordering.Application.Common.Service;
using Ordering.Domain.Entities;
using System.Data;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDapperService _dapperService;
        public OrderRepository(IDapperService dapperService) 
            => _dapperService = dapperService;

        public async Task<List<Order>> GetOrdersByUserName(string userName)
        {
            var parmeters = new DynamicParameters();
            parmeters.Add("@UserName", userName, DbType.String);

            return await _dapperService.ExecuteQueryWithParam<Order, DynamicParameters>("GetOrdersByUserName", parmeters);
        }

        public async Task<Order> AddOrder(Order order)
        {
            var parmeters = new DynamicParameters();
            parmeters.Add("@UserName", order.UserName, DbType.String);
            parmeters.Add("@TotalPrice", order.TotalPrice, DbType.Decimal);
            parmeters.Add("@FirstName", order.FirstName, DbType.String);
            parmeters.Add("@LastName", order.LastName, DbType.String);
            parmeters.Add("@EmailAddress", order.EmailAddress, DbType.String);
            parmeters.Add("@PhoneNumber", order.PhoneNumber, DbType.String);
            parmeters.Add("@Address", order.Address, DbType.String);
            parmeters.Add("@City", order.City, DbType.String);
            parmeters.Add("@State", order.State, DbType.String);
            parmeters.Add("@ZipCode", order.ZipCode, DbType.String);
            parmeters.Add("@CardName", order.CardName, DbType.String);
            parmeters.Add("@CardNumber", order.CardNumber, DbType.String);
            parmeters.Add("@CVV", order.CVV, DbType.String);
            parmeters.Add("@Expiration", order.Expiration, DbType.String);
            parmeters.Add("@CreatedBy", order.CreatedBy, DbType.String);

            var newId = await _dapperService.ExecuteNonQuery<Guid, DynamicParameters>("InsertOrder", parmeters);
            order.Id = newId;
            return order;
        }

        public async Task UpdateOrder(Order order)
        {
            var parmeters = new DynamicParameters();
            parmeters.Add("@Id", order.Id, DbType.Guid);
            parmeters.Add("@UserName", order.UserName, DbType.String);
            parmeters.Add("@TotalPrice", order.TotalPrice, DbType.Decimal);
            parmeters.Add("@FirstName", order.FirstName, DbType.String);
            parmeters.Add("@LastName", order.LastName, DbType.String);
            parmeters.Add("@EmailAddress", order.EmailAddress, DbType.String);
            parmeters.Add("@PhoneNumber", order.PhoneNumber, DbType.String);
            parmeters.Add("@Address", order.Address, DbType.String);
            parmeters.Add("@City", order.City, DbType.String);
            parmeters.Add("@State", order.State, DbType.String);
            parmeters.Add("@ZipCode", order.ZipCode, DbType.String);
            parmeters.Add("@CardName", order.CardName, DbType.String);
            parmeters.Add("@CardNumber", order.CardNumber, DbType.String);
            parmeters.Add("@CVV", order.CVV, DbType.String);
            parmeters.Add("@Expiration", order.Expiration, DbType.String);
            parmeters.Add("@UpdatedBy", order.UpdatedBy, DbType.String);

            await _dapperService.ExecuteNonQuery<DynamicParameters>("UpdateOrder", parmeters);
        }

        public async Task DeleteOrder(Guid id)
        {
            var parmeters = new DynamicParameters();
            parmeters.Add("@Id", id, DbType.Guid);

            await _dapperService.ExecuteNonQuery<DynamicParameters>("DeleteOrder", parmeters);
        }
    }
}
