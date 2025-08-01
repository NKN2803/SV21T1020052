using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020052.BusinessLayers;
using SV21T1020052.DomainModels;
using SV21T1020052.Shop.Appcodes;
using SV21T1020052.Shop.Models;

namespace SV21T1020052.Shop.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private const int DefaultPageSize = 15;

        public IActionResult Index(ProductSearchInput? searchInput, int? categoryId)
        {
            searchInput ??= new ProductSearchInput
            {
                Page = 1,
                PageSize = DefaultPageSize,
                SearchValue = "",
                CategoryID = categoryId ?? CategoryDataService.List().OrderBy(c => c.CategoryID).First().CategoryID,
                MinPrice = 0,
                MaxPrice = 0,
                SortOrder = "default"
            };

            searchInput.PageSize = DefaultPageSize;
            if (searchInput.Page < 1) searchInput.Page = 1;

            int rowCount = 0;
            var data = ProductDataService.ListProducts(
                out rowCount,
                searchInput.Page,
                searchInput.PageSize,
                searchInput.SearchValue ?? "",
                searchInput.CategoryID, 0,
                searchInput.MinPrice > 0 ? searchInput.MinPrice : 0,
                searchInput.MaxPrice > 0 ? searchInput.MaxPrice : int.MaxValue
            );

            data = searchInput.SortOrder switch
            {
                "price_asc" => data.OrderBy(p => p.Price).ToList(),
                "price_desc" => data.OrderByDescending(p => p.Price).ToList(),
                "name_asc" => data.OrderBy(p => p.ProductName).ToList(),
                "name_desc" => data.OrderByDescending(p => p.ProductName).ToList(),
                _ => data
            };

            var model = new ProductModel
            {
                SearchInput = searchInput,
                Categories = CategoryDataService.List(),
                Products = data,
                TotalPages = (int)Math.Ceiling(rowCount / (double)searchInput.PageSize),
                DefaultPageSize = DefaultPageSize
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var product = ProductDataService.GetProduct(id);
            if (product == null)
                return NotFound();

            int rowCount = 0;
            var relatedProducts = ProductDataService.ListProducts(
                out rowCount, 1, 4, "", product.CategoryID)
                .Where(p => p.CategoryID == product.CategoryID && p.ProductID != product.ProductID)
                .Take(4)
                .ToList();

            var model = new ProductDetailModel
            {
                Product = product,
                RelatedProducts = relatedProducts
            };

            return View(model);
        }
    }
}
