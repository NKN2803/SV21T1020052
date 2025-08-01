using SV21T1020052.DomainModels;

namespace SV21T1020052.DataLayers
{
    public interface IProductDAL
    {
        List<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0,
                            decimal maxPrice = 0);

        int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);

        Product? Get(int productID);

        int Add(Product data);

        bool Update(Product data);

        bool Delete(int productID);

        bool InUsed(int productID);

        List<ProductPhoto> ListPhotos(int productID);

        ProductPhoto? GetPhoto(long photoID);

        long AddPhoto(ProductPhoto data);

        bool UpdatePhoto(ProductPhoto data);

        bool DeletePhoto(long photoID);

        List<ProductAttribute> ListAttributes(int productID);

        ProductAttribute? GetAttribute(long attributeID);

        long AddAttribute(ProductAttribute data);

        bool UpdateAttribute(ProductAttribute data);

        bool DeleteAttribute(long attributeID);

        public long GetConflictingAttributeID(int productID, int displayOrder, long attributeID);

        public long GetConflictingPhotoID(int productID, int displayOrder, long photoID);
    }
}
