﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020052.BusinessLayers;
using SV21T1020052.DomainModels;
using SV21T1020052.Web.AppCodes;
using SV21T1020052.Web.Models;
using SV21T1020052.Web.Search;

namespace SV21T1020052.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMIN},{WebUserRoles.MANAGER}")]
    public class SupplierController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string SUPPLIER_SEARCH_CONDITION = "SupplierSearchCondition";
        public IActionResult Index()
        {
            PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(SUPPLIER_SEARCH_CONDITION);
            if (condition == null)
            {
                condition = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(condition);
        }

        public IActionResult Search(PaginationSearchInput condition)
        {
            int rowCount;
            var data = CommonDataService.ListOfSuppliers(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "");
            SupplierSearchResult model = new SupplierSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            ApplicationContext.SetSessionData(SUPPLIER_SEARCH_CONDITION, condition);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            var data = new Supplier()
            {
                SupplierID = 0
            };
            return View("Edit", data);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhà cung cấp";
            var data = CommonDataService.GetSupplier(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }


        [HttpPost]
        public IActionResult Save(Supplier data) //int customerID, string CustomerName, ....
        {
            ViewBag.Title = data.SupplierID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật thông tin nhà cung cấp";
            //TODO: Kiểm soát dữ liệu đầu vào (TaskList)
            if (string.IsNullOrWhiteSpace(data.SupplierName))
                ModelState.AddModelError(nameof(data.SupplierName), "Tên nhà cung cấp không được để trống");
            if (string.IsNullOrWhiteSpace(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được để trống");
            if (string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập điện thoại của nhà cung cấp");
            if (string.IsNullOrWhiteSpace(data.Email))
                ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập Email của nhà cung cấp");
            if (string.IsNullOrWhiteSpace(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Vui lòng nhập địa chỉ của nhà cung cấp");
            if (string.IsNullOrWhiteSpace(data.Provice))
                ModelState.AddModelError(nameof(data.Provice), "Bạn chưa chọn tỉnh/thành của nhà cung cấp");

            if (!ModelState.IsValid)
            {
                return View("Edit", data); //Trả dữ liệu về cho View, kèm theo thông báo lỗi ở trong ModelState
            }

            if (data.SupplierID == 0)
            {
                int id = CommonDataService.AddSupplier(data);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
                    return View("Edit", data);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateSupplier(data);
                if (!result)
                {
                    ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
                    return View("Edit", data);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteSupplier(id);
                return RedirectToAction("Index");
            }

            var data = CommonDataService.GetSupplier(id);
            if (data == null)
                return RedirectToAction("Index");

            return View(data);
        }
    }
}
