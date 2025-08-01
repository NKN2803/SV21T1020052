using Dapper;
using SV21T1020052.DomainModels;
using System.Data;

namespace SV21T1020052.DataLayers.SQLServer
{
    public class CustomerDAL : BaseDAL, ICommonDAL<Customer>
    {
        public CustomerDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Customer data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Customers where Email = @Email)
                                select -1;
                            else
                                begin
                                    insert into Customers(CustomerName, ContactName, Province, Address, Phone, Email, IsLocked)
                                    values (@CustomerName, @ContactName, @Province, @Address, @Phone, @Email, @IsLocked);
                                    select SCOPE_IDENTITY();
                                end";
                var parameters = new
                {
                    CustomerName = data.CustomerName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    IsLocked = data.IsLocked,
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
                            from	Customers
                            where	(CustomerName like @searchValue) or (ContactName like @searchValue)";
                var parameters = new 
                {
                    searchValue    
                };
                count = connection.ExecuteScalar<int>(sql: sql, param:  parameters, commandType: CommandType.Text);   
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from Customers where CustomerID = @CustomerID";
                var parameters = new
                {
                    CustomerID = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0; //=0 ko xóa đc dữ liệu
                connection.Close();
            }
            return result;
        }

        public Customer? Get(int id)
        {
            Customer? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Customers where CustomerID = @CustomerID";
                var parameters = new
                {
                    CustomerID = id
                };
                data = connection.QueryFirstOrDefault<Customer>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Orders where CustomerID = @CustomerID)
	                            select 1
                            else
	                            select 0";
                var parameters = new
                {
                    CustomerID = id
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public List<Customer> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Customer> data = new List<Customer>();
            searchValue = $"%{searchValue}%"; //tìm kiếm tương đối với like

            using (var connection = OpenConnection())
            {
                var sql = @"select *
                          from	(
		                            select	*,
				                            row_number() over (order by CustomerName) as RowNumber
		                            from	Customers
		                            where	(CustomerName like @searchValue) or (ContactName like @searchValue)
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
                data = connection.Query<Customer>(sql: sql, param: parameter, commandType: System.Data.CommandType.Text).ToList(); //truy vấn dữ liệu bằng cách cho connection truy vấn
                                                                                                                                   //dữ liệu về 1 Customer với câu lệnh sql là gì, tham số là gì và loại câu lệnh Text
                                                                                                                                   //(tính năng này là của Dapper giúp code nhanh hơn)
                connection.Close();
            }
            return data;
        }

        public bool Update(Customer data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Customers where CustomerID <> @CustomerID and Email = @Email)
                                begin
                                    update Customers
                                    set CustomerName = @CustomerName,
	                                    ContactName  = @ContactName,
	                                    Province = @Province,
	                                    Address = @Address,
	                                    Phone = @Phone,
	                                    Email = @Email,
	                                    IsLocked = @IsLocked
                                    where CustomerID = @CustomerID
                                end";
                var parameters = new
                {
                    CustomerID = data.CustomerID,
                    CustomerName = data.CustomerName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province,
                    Address = data.Address,
                    Phone = data.Phone,
                    Email = data.Email,
                    IsLocked = data.IsLocked,

                    
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0; //=0 ko có dòng nào đc cập nhật
                connection.Close();
            }
            return result;
        }
    }
}
