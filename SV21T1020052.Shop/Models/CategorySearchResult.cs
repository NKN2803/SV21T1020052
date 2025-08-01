using SV21T1020052.DomainModels;

namespace SV21T1020052.Shop.Models;

public class CategorySearchResult : PaginationSearchResult
{
    public required List<Category> Data { get; set; }
}
