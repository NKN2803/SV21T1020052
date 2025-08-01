using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV21T1020052.BusinessLayers;
using SV21T1020052.DataLayers.SQLServer;
using SV21T1020052.DomainModels;
using SV21T1020052.DataLayers.SQLServer;
using SV21T1020052.DomainModels;

namespace SV21T1020052.BusinessLayers
{
    public class CategoryDataService
    {
        private static readonly CategoryDAL categoryDB;
        static CategoryDataService()
        {
            categoryDB = new CategoryDAL(Configuration.ConnectionString);
        }

        /// <summary>
        /// Lấy danh sách category với số lượng sản phẩm
        /// </summary>
        public static List<Category> ListWithProductCount()
        {
            return categoryDB.ListWithProductCount();
        }

        public static List<Category> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            return categoryDB.List(page, pageSize, searchValue);
        }

        public static Category? Get(int categoryId)
        {
            return categoryDB.Get(categoryId);
        }

        public static int Add(Category data)
        {
            return categoryDB.Add(data);
        }

        public static bool Update(Category data)
        {
            return categoryDB.Update(data);
        }

        public static bool Delete(int categoryId)
        {
            if (categoryDB.InUsed(categoryId))
            {
                return false;
            }
            return categoryDB.Delete(categoryId);
        }

        public static int Count(string searchValue = "")
        {
            return categoryDB.Count(searchValue);
        }
    }
}