using Dapper;
using SV21T1020052.DomainModels;
using System.Data;

namespace SV21T1020052.DataLayers.SQLServer
{
    public class CategoryDAL : BaseDAL, ICommonDAL<Category>
    {
        public CategoryDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Category data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Categories where CategoryName = @CategoryName)
                                select -1;
                            else
                                begin
                                    insert into Categories(CategoryName, Description)
                                    values (@CategoryName, @Description);
                                    select SCOPE_IDENTITY();
                                end";
                var parameters = new
                {
                    CategoryName = data.CategoryName ?? "",
                    Description = data.Description ?? ""
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            searchValue = $"%{searchValue}%"; // "%" + searchValue + "%" (viết gộp lại thì trước chuỗi thêm $)
            using (var connection = OpenConnection())
            {
                var sql = @" select	Count(*)
                            from	Categories
                            where	(CategoryName like @searchValue)";
                var parameters = new
                {
                    searchValue
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from Categories where CategoryID = @CategoryID";
                var parameters = new
                {
                    CategoryID = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0; //=0 ko xóa đc dữ liệu
                connection.Close();
            }
            return result;
        }

        public Category? Get(int id)
        {
            Category? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Categories where CategoryID = @CategoryID";
                var parameters = new
                {
                    CategoryID = id
                };
                data = connection.QueryFirstOrDefault<Category>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Products where CategoryID = @CategoryID)
	                            select 1
                            else
	                            select 0";
                var parameters = new
                {
                    CategoryID = id
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public List<Category> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Category> data = new List<Category>();
            searchValue = $"%{searchValue}%"; //tìm kiếm tương đối với like

            using (var connection = OpenConnection())
            {
                var sql = @"select *
                          from	(
		                            select	*,
				                            row_number() over (order by CategoryName) as RowNumber
		                            from	Categories
		                            where	(CategoryName like @searchValue)
		                            ) as t
                            where	(@pageSize = 0)
	                            or	(RowNumber between (@page - 1) * @pageSize and @page * @pageSize)
                            order by RowNumber "; //câu lệnh sql (chuỗi viết trên nhiều dòng thì thêm @
                var parameter = new
                {
                    page,
                    pageSize, //với trường hợp hai bên giống nhau thì viết như này (khuyến cáo nên viết gọn) 
                    searchValue = searchValue // đây là cách viết tường minh bên phải là tham số đầu vào của hàm (giá trị truyền tham số), bên trái là tên tham số trong câu lệnh sql
                };//tham số cho câu lệnh sql
                data = connection.Query<Category>(sql: sql, param: parameter, commandType: System.Data.CommandType.Text).ToList(); //truy vấn dữ liệu bằng cách cho connection truy vấn
                                                                                                                                  //dữ liệu về 1 Category với câu lệnh sql là gì, tham số là gì và loại câu lệnh Text
                                                                                                                                  //(tính năng này là của Dapper giúp code nhanh hơn)
                connection.Close();
            }
            return data;
        }

        public List<Category> ListWithProductCount()
        {
            throw new NotImplementedException();
        }

        public bool Update(Category data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Categories where CategoryID <> @CategoryID and CategoryName = @CategoryName)
                                begin
                                    update Categories
                                    set CategoryName = @CategoryName,
	                                    Description  = @Description
                                    where CategoryID = @CategoryID
                                end";
                var parameters = new
                {
                    CategoryID = data.CategoryID,
                    CategoryName = data.CategoryName ?? "",
                    Description = data.Description ?? ""
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0; //=0 ko có dòng nào đc cập nhật
                connection.Close();
            }
            return result;
        }
    }
}
