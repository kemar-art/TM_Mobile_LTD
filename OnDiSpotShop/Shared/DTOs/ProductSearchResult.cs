﻿namespace OnDiSpotShop.Shared.DTOs
{
    public class ProductSearchResult
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
