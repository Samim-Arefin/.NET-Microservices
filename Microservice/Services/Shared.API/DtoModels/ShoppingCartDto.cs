namespace Shared.API.DtoModels
{
    public class ShoppingCartDto
    {
        public string UserName { get; set; }
        public List<ShoppingCartItemDto> ShoppingCartItems { get; set; } = new();
        public decimal TotalPrice
        {
            get
            {
                return ShoppingCartItems.Sum(x => x.Price);
            }
        }
    }
}
