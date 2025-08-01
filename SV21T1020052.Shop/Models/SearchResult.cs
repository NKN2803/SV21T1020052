namespace SV21T1020052.Shop.Models
{
    public class SearchResult
    {
        public bool Success { get; set; }
        public List<SearchProductItem> Products { get; set; } = new();
        public List<SearchProductItem> RelatedProducts { get; set; } = new();
    }

    public class SearchProductItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public string ImageUrl { get; set; } = "";
        public int CategoryID { get; set; }
        public bool IsOnSale => OriginalPrice > Price;
    }
}
