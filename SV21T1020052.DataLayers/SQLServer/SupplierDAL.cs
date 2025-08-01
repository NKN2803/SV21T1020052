using Dapper;
using SV21T1020052.DomainModels;
using System.Data;

namespace SV21T1020052.DataLayers.SQLServer
{
    public class SupplierDAL : BaseDAL, ICommonDAL<Supplier>
    {
        public SupplierDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Supplier data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Suppliers where Email = @Email)
                                select -1;
                            else
                                begin
                                    insert into Suppliers(SupplierName, ContactName, Provice, Address, Phone, Email)
                                    values (@SupplierName, @ContactName, @Province, @Address, @Phone, @Email);
                                    select SCOPE_IDENTITY();
                                end";
                var parameters = new
                {
                    SupplierName = data.SupplierName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Provice ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? ""
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
                            from	Suppliers
                            where	(SupplierName like @searchValue) or (ContactName like @searchValue)";
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
                var sql = @"delete from Suppliers where SupplierID = @SupplierID";
                var parameters = new
                {
                    SupplierID = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0; //=0 ko xóa đc dữ liệu
                connection.Close();
            }
            return result;
        }

        public Supplier? Get(int id)
        {
            Supplier? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Suppliers where SupplierID = @SupplierID";
                var parameters = new
                {
                    SupplierID = id
                };
                data = connection.QueryFirstOrDefault<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                    var sql = @"if exists(select * from Products where SupplierID = @SupplierID)
	                                select 1
                                else
	                                select 0";
                var parameters = new
                {
                    SupplierID = id
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public List<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Supplier> data = new List<Supplier>();
            searchValue = $"%{searchValue}%"; //tìm kiếm tương đối với like

            using (var connection = OpenConnection())
            {
                var sql = @"select *
                          from	(
		                            select	*,
				                            row_number() over (order by SupplierName) as RowNumber
		                            from	Suppliers
		                            where	(SupplierName like @searchValue) or (ContactName like @searchValue)
		                            ) as t
                            where	(@pageSize = 0)
	                            or	(RowNumber between (@page - 1) * @pageSize and @page * @pageSize)
                            order by RowNumber "; //câu lệnh sql (chuỗi viết trên nhiều dòng thì thêm @)
                var parameter = new
                {
                    page,
                    pageSize, //với trường hợp hai bên giống nhau thì viết như này (khuyến cáo nên viết gọn) 
                    searchValue = searchValue // đây là cách viết tường minh bên phải là tham số đầu vào của hàm (giá trị truyền tham số), bên trái là tên tham số trong câu lệnh sql
                };//tham số cho câu lệnh sql
                data = connection.Query<Supplier>(sql: sql, param: parameter, commandType: System.Data.CommandType.Text).ToList(); //truy vấn dữ liệu bằng cách cho connection truy vấn
                                                                                                                                   //dữ liệu về 1 Supplier với câu lệnh sql là gì, tham số là gì và loại câu lệnh Text
                                                                                                                                   //(tính năng này là của Dapper giúp code nhanh hơn)
                connection.Close();
            }
            return data;
        }

        public bool Update(Supplier data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Suppliers where SupplierID <> @SupplierID and Email = @Email)
                                begin
                                    update Suppliers
                                    set SupplierName = @SupplierName,
	                                    ContactName  = @ContactName,
	                                    Provice = @Province,
	                                    Address = @Address,
	                                    Phone = @Phone,
	                                    Email = @Email
                                    where SupplierID = @SupplierID
                                end";
                var parameters = new
                {
                    SupplierID = data.SupplierID,
                    SupplierName = data.SupplierName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Provice,
                    Address = data.Address,
                    Phone = data.Phone,
                    Email = data.Email
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0; //=0 ko có dòng nào đc cập nhật
                connection.Close();
            }
            return result;
        }
    }
}
