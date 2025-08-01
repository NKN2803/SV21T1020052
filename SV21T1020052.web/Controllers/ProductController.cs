using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020052.BusinessLayers;
using SV21T1020052.DomainModels;
using SV21T1020052.Web.AppCodes;
using SV21T1020052.BusinessLayers;
using SV21T1020052.DomainModels;
using SV21T1020052.Web.AppCodes;
using SV21T1020052.Web.Models;
using SV21T1020052.Web.Search;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020052.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMIN},{WebUserRoles.MANAGER}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";
        public IActionResult Index()
        {
            ProductSearchInput? condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION);
            if (condition == null)
            {
                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    supplierID = 0,
                    categoryID = 0,
                    minPrice = 0,
                    maxPrice = 0
                };
            }
            return View(condition);
        }

        public IActionResult Search(ProductSearchInput condition)
        {
            int rowCount;
            var data = ProductDataService.ListProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "", condition.categoryID, condition.supplierID, condition.minPrice, condition.maxPrice);
            ProductSearchResult model = new ProductSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                categoryID = condition.categoryID,
                supplierID = condition.supplierID,
                minPrice = condition.minPrice,
                maxPrice = condition.maxPrice,
                Data = data
            };

            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);

            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            var data = new Product()
            {
                ProductID = 0,
                IsSelling = true
            };
            return View("Edit", data);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
            var data = ProductDataService.GetProduct(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }

        [HttpPost]
        public IActionResult Save(Product data, IFormFile? uploadPhoto, ProductPhoto productPhoto, ProductAttribute productAttribute)
        {
            ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";

            if (string.IsNullOrWhiteSpace(data.ProductName))
                ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng không được để trống");
            if (data.CategoryID == 0)
                ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng nhập loại hàng của mặt hàng");
            if (data.SupplierID == 0)
                ModelState.AddModelError(nameof(data.SupplierID), "Vui lòng nhập nhà cung cấp của mặt hàng");
            if (string.IsNullOrWhiteSpace(data.Unit))
                ModelState.AddModelError(nameof(data.Unit), "Vui lòng nhập đơn vị tính của mặt hàng");
            if (data.Price <= 0)
                ModelState.AddModelError(nameof(data.SupplierID), "Vui lòng nhập giá sản phẩm lớn hơn 0");
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}-{uploadPhoto.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\products", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
                productPhoto.Photo = filePath;
            }


            if (!ModelState.IsValid)
            {
                return View("Edit", data); //Trả dữ liệu về cho View, kèm theo thông báo lỗi ở trong ModelState
            }

            if (data.ProductID == 0)
            {
                int id = ProductDataService.AddProduct(data);
                //if (id <= 0)
                //{
                //   ModelState.AddModelError(nameof(data.ProductName), "Tên sản phẩm bị trùng");
                //   return View("Edit", data);
                //}
                return RedirectToAction("Edit", new { id });
            }
            else
            {
                //bool result = ProductDataService.UpdateProduct(data);
                //if (!result)
                //{
                //    ModelState.AddModelError(nameof(data.ProductName), "Tên sản phẩm bị trùng");
                //    return View("Edit", data);
                //}
                ProductDataService.UpdateProduct(data);
                return RedirectToAction("Edit", new { id = data.ProductID });
            }
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }

            var data = ProductDataService.GetProduct(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }

        public IActionResult Photo(int id = 0, string method = "", int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = " Bổ sung ảnh cho mặt hàng";
                    var data = new ProductPhoto()
                    {
                        ProductID = id,
                        PhotoID = 0,
                        IsHidden = false,
                        DisplayOrder = 1
                    };
                    return View(data);

                case "edit":
                    ViewBag.Title = "Thay đổi ảnh của mặt hàng";
                    var ProductPhoto = ProductDataService.GetPhoto(photoId);
                    if (ProductPhoto == null)
                    {
                        return RedirectToAction("Edit", new { id = id });
                    }
                    return View(ProductPhoto);

                case "delete":
                    bool deleteResult = ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        public IActionResult SavePhoto(ProductPhoto productPhoto, IFormFile uploadPhoto)
        {
            ViewBag.Title = productPhoto.PhotoID == 0 ? "Bổ sung ảnh của mặt hàng" : "Thay đổi ảnh của mặt hàng";

            // Validation
            if (uploadPhoto == null && productPhoto.PhotoID == 0)
                ModelState.AddModelError(nameof(productPhoto.Photo), "Vui lòng chọn ảnh của mặt hàng");

            if (string.IsNullOrWhiteSpace(productPhoto.Description))
                ModelState.AddModelError(nameof(productPhoto.Description), "Vui lòng nhập mô tả của sản phẩm");

            if (productPhoto.DisplayOrder <= 0)
                ModelState.AddModelError(nameof(productPhoto.DisplayOrder), "Vui lòng nhập thứ tự hiển thị lớn hơn 0");

            if (productPhoto.PhotoID == 0
                    && ProductDataService.ExistsDisplayOrderPhoto(
                            productPhoto.ProductID,
                            productPhoto.DisplayOrder,
                            productPhoto.PhotoID
                        ))
                ModelState.AddModelError(nameof(productPhoto.DisplayOrder), "Vui lòng nhập thứ tự hiện thị không trùng");

            if (!ModelState.IsValid)
                return View("Photo", productPhoto);

            // Xử lý upload ảnh
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}-{uploadPhoto?.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\products", fileName);
                productPhoto.Photo = fileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto?.CopyTo(stream);
                }
            }

            if (productPhoto.PhotoID == 0)
            {
                long id = ProductDataService.AddPhoto(productPhoto);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(productPhoto.Photo), "Vui lòng chọn lại ảnh của mặt hàng");
                    return View("Photo", productPhoto);
                }
            }
            else
            {
                long duplicatePhotoID = ProductDataService.GetConflictingPhotoID(
                    productPhoto.ProductID,
                    productPhoto.DisplayOrder,
                    productPhoto.PhotoID
                );
                if (duplicatePhotoID > 0)
                {
                    // Lấy ra dữ liệu ảnh đang cập nhật trong CSDL
                    ProductPhoto? updatingPhoto = ProductDataService.GetPhoto(productPhoto.PhotoID);

                    // Lấy ra dữ liệu ảnh bị trùng
                    ProductPhoto? duplicatePhoto = ProductDataService.GetPhoto(duplicatePhotoID);

                    // Gán VTHT của ảnh bị trùng bằng VTHT ảnh đang cập nhật
                    duplicatePhoto.DisplayOrder = updatingPhoto.DisplayOrder;

                    // Cập nhật dữ liệu ảnh bị trùng
                    ProductDataService.UpdatePhoto(duplicatePhoto);
                }

                ProductDataService.UpdatePhoto(productPhoto);
            }

            return RedirectToAction("Edit", new { id = productPhoto.ProductID });
        }

        public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    ProductAttribute addProductAttribute = new() { AttributeID = attributeId, ProductID = id, DisplayOrder = 1 };
                    return View(addProductAttribute);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính của mặt hàng";
                    ProductAttribute? editProductAttribute = ProductDataService.GetAttribute(attributeId);
                    if (editProductAttribute == null)
                    {
                        return RedirectToAction("Edit", new { id = id });
                    }
                    return View(editProductAttribute);
                case "delete":
                    ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        public IActionResult SaveAttribute(ProductAttribute productAttribute)
        {
            ViewBag.Title = productAttribute.AttributeID == 0 ? "Bổ sung thuộc tính của mặt hàng" : "Thay đổi thuộc tính của mặt hàng";

            if (string.IsNullOrWhiteSpace(productAttribute.AttributeName))
                ModelState.AddModelError(nameof(productAttribute.AttributeName), "Tên thuộc tính không được để trống");

            if (string.IsNullOrWhiteSpace(productAttribute.AttributeValue))
                ModelState.AddModelError(nameof(productAttribute.AttributeValue), "Giá trị thuộc tính không được để trống");

            if (productAttribute.DisplayOrder <= 0)
                ModelState.AddModelError(nameof(productAttribute.DisplayOrder), "Vui lòng nhập thứ tự hiển thị của thuộc tính phải lớn hơn 0");

            if (productAttribute.AttributeID == 0
                    && ProductDataService.GetConflictingAttributeID(
                        productAttribute.ProductID,
                        productAttribute.DisplayOrder,
                        productAttribute.AttributeID) > 0
                )
                ModelState.AddModelError(nameof(productAttribute.DisplayOrder), "Vui lòng nhập thứ tự hiện thị không trùng");

            if (ProductDataService.ExistsAttribute(productAttribute.ProductID, productAttribute.AttributeName, productAttribute.AttributeValue, productAttribute.AttributeID))
                ModelState.AddModelError(nameof(productAttribute.AttributeValue), "Vui lòng nhập giá trị thuộc tính không trùng");

            if (ModelState.IsValid == false)
                return View("Attribute", productAttribute);

            if (productAttribute.AttributeID == 0)
            {
                long AttributeID = ProductDataService.AddAttribute(productAttribute);
                if (AttributeID <= 0)
                {
                    return View("Attribute", productAttribute);
                }
            }
            else
            {
                long duplicateAttributeID = ProductDataService.GetConflictingAttributeID(
                    productAttribute.ProductID,
                    productAttribute.DisplayOrder,
                    productAttribute.AttributeID
                );
                if (duplicateAttributeID > 0)
                {
                    // Lấy ra dữ liệu thuộc tính đang cập nhật trong CSDL
                    ProductAttribute? updatingAttribute = ProductDataService.GetAttribute(productAttribute.AttributeID);

                    // Lấy ra thuộc tính bị trùng
                    ProductAttribute? duplicateAttribute = ProductDataService.GetAttribute(duplicateAttributeID);

                    // Gán VTHT của thuộc tính bị trùng bằng thuộc tính đang cập nhật
                    duplicateAttribute.DisplayOrder = updatingAttribute.DisplayOrder;

                    // Cập nhật thuộc tính bị trùng
                    ProductDataService.UpdataAttribute(duplicateAttribute);
                }
                ProductDataService.UpdataAttribute(productAttribute);
            }

            return RedirectToAction("Edit", new { id = productAttribute.ProductID });
        }
    }
}