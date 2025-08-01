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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public const int PAGE_SIZE = 10;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var condition = GetProductSearchCondition();
            var categories = GetCategories();
            var products = GetCategoryProducts(categories);

            var model = new HomeModel
            {
                Categories = categories,
                Products = products,
                ProductSearch = condition
            };

            return View(model);
        }

        private ProductSearchInput GetProductSearchCondition()
        {
            var condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION);
            if (condition == null)
            {
                condition = new ProductSearchInput
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    MinPrice = 0,
                    MaxPrice = 0
                };
            }
            return condition;
        }

        private List<HomeCategory> GetCategories()
        {
            return new List<HomeCategory>
            {
                CreateCategory(1, "May mặc - Thời trang", "fas fa-tshirt", 1),
                CreateCategory(2, "Mỹ phẩm", "fas fa-spa", 2),
                CreateCategory(3, "Điện tử", "fas fa-tv", 3),
                CreateCategory(4, "Hàng gia dụng", "fas fa-home", 4),
                CreateCategory(5, "Dành cho bé", "fas fa-baby", 5),
                CreateCategory(6, "Xe máy", "fas fa-motorcycle", 6),
                CreateCategory(7, "Ô tô", "fas fa-car", 7),
                CreateCategory(9, "Đồ chơi - Phụ kiện", "fas fa-gamepad", 9),
                CreateCategory(10, "Bàn ghế - Nội thất", "fas fa-couch", 10)
            };
        }

        private HomeCategory CreateCategory(int categoryId, string categoryName, string iconClass, int categoryIdToCount)
        {
            return new HomeCategory
            {
                CategoryID = categoryId,
                CategoryName = categoryName,
                IconClass = iconClass,
                ProductCount = ProductDataService.CountProductsByCategory(categoryIdToCount)
            };
        }

        private List<Product> GetCategoryProducts(List<HomeCategory> categories)
        {
            var allProducts = new List<Product>();

            foreach (var category in categories)
            {
                var categoryProducts = ProductDataService.ListProducts(
                    out int rowCount,
                    1, // Page luôn là 1
                    2, // CHỈ LẤY 2 sản phẩm cho Home
                    "",
                    category.CategoryID
                );

                if (categoryProducts != null)
                {
                    allProducts.AddRange(categoryProducts);
                }
            }

            return allProducts;
        }

        public IActionResult Detail(int id)
        {
            var product = ProductDataService.GetProduct(id);
            if (product == null)
                return NotFound();

            var relatedProducts = GetRelatedProducts(product);

            var model = new ProductDetailModel
            {
                Product = product,
                RelatedProducts = relatedProducts
            };

            return View(model);
        }

        private List<Product> GetRelatedProducts(Product product)
        {
            int rowCount = 0;
            return ProductDataService.ListProducts(
                    out rowCount,
                    1, 4, "",
                    product.CategoryID)
                .Where(p => p.CategoryID == product.CategoryID && p.ProductID != product.ProductID)
                .Take(4)
                .ToList();
        }
    }
}
