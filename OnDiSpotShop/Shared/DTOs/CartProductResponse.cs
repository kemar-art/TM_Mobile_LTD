using OnDiSpotShop.Shared.Modles;

namespace OnDiSpotShop.Shared.DTOs
{
    public class CartProductResponse : Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
