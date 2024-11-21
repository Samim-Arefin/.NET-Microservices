namespace Shared.API.DtoModels
{
    public class CouponDto
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Amount { get; set; } = 0;
    }
}
