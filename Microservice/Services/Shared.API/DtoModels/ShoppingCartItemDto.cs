﻿namespace Shared.API.DtoModels
{
    public class ShoppingCartItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
