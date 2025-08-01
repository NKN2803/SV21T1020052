using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV21T1020052.BusinessLayers;
using SV21T1020052.DataLayers.SQLServer;
using SV21T1020052.DomainModels;
using SV21T1020052.DataLayers;
using Sv21T1020052.DataLayers.SQL_Server;

namespace SV21T1020052.BusinessLayers
{
    /// <summary>
    /// Các nghiệp vụ quản lý hàng hóa
    /// </summary>
    public class ProductDataService
    {
        private static readonly ProductDAL productDB;

        /// <summary>
        /// Ctor
        /// </summary>
        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            // Kiểm tra nếu categoryId không được chỉ định (giá trị bằng 0), thì lấy CategoryID của danh mục đầu tiên
            if (categoryId == 0)
            {
                // Giả sử bạn có phương thức List() trong CategoryDataService để lấy danh sách các danh mục
                var categories = CategoryDataService.List(); // Lấy danh sách tất cả các danh mục
                categoryId = categories.OrderBy(c => c.CategoryName).FirstOrDefault()?.CategoryID ?? 0; // Lấy CategoryID của danh mục đầu tiên
            }

            rowCount = productDB.Count(searchValue, categoryId, supplierId, minPrice, maxPrice);
            return productDB.List(page, pageSize, searchValue, categoryId, supplierId, minPrice, maxPrice);
        }

        // Đếm số lượng sản phẩm
        public static int CountProductsByCategory(int categoryId)
        {
            return productDB.CountByCategory(categoryId);
        }
        public static Product? GetProduct(int productID)
        {
            return productDB.Get(productID);
        }

        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }

        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }


        public static bool DeleteProduct(int productID)
        {
            return productDB.Delete(productID);
        }

        public static bool InUsedProduct(int productID)
        {
            return productDB.InUsed(productID);
        }

        public static List<ProductPhoto> ListPhotos(int productID)
        {
            return productDB.ListPhotos(productID);
        }

        public static ProductPhoto? GetPhoto(long photoID)
        {
            return productDB.GetPhoto(photoID);
        }

        public static long AddPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }

        public static bool UpdatePhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }

        public static bool DeletePhoto(long photoID)
        {
            return productDB.DeletePhoto(photoID);
        }

        public static List<ProductAttribute> ListAttributes(int productID)
        {
            return productDB.ListAttributes(productID);
        }

        public static ProductAttribute? GetAttribute(long attributeID)
        {
            return productDB.GetAttribute(attributeID);
        }

        public static long AddAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }

        public static bool UpdateAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }

        public static bool DeleteAttribute(long attributeID)
        {
            return productDB.DeleteAttribute(attributeID);
        }

        public static long GetConflictingAttributeID(int ProductID, int DisplayOrder, long AttributeID)
        {
            return productDB.GetConflictingAttributeID(ProductID, DisplayOrder, AttributeID);
        }

        public static long GetConflictingPhotoID(int productID, int displayOrder, long photoID)
        {
            return productDB.GetConflictingPhotoID(productID, displayOrder, photoID);
        }

        public static bool ExistsAttribute(int productID, string attributeName, string attributeValue, long attributeID)
        {
            return productDB.ExistsAttribute(productID, attributeName, attributeValue, attributeID);
        }

        public static bool ExistsDisplayOrderPhoto(int ProductID, int DisplayOrder, long PhotoID)
        {
            return productDB.ExistsDisplayOrderPhoto(ProductID, DisplayOrder, PhotoID);
        }

        public static void UpdataAttribute(ProductAttribute duplicateAttribute)
        {
            throw new NotImplementedException();
        }
    }
}
