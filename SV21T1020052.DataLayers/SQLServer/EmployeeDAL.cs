using Dapper;
using SV21T1020052.DomainModels;
using System.Data;

namespace SV21T1020052.DataLayers.SQLServer
{
    public class EmployeeDAL : BaseDAL, ICommonDAL<Employee>
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Employee data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Employees where Email = @Email)
                                select -1;
                            else
                                begin
                                    insert into Employees(FullName, BirthDate, Address, Phone, Email, Photo, IsWorking)
                                    values (@FullName, @BirthDate, @Address, @Phone, @Email, @Photo, @IsWorking);
                                    select SCOPE_IDENTITY();
                                end";
                var parameters = new
                {
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Photo = data.Photo  ?? "",
                    IsWorking = data.IsWorking
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
                            from	Employees
                            where	(FullName like @searchValue)";
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
                var sql = @"delete from Employees where EmployeeID = @EmployeeID";
                var parameters = new
                {
                    EmployeeID = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0; //=0 ko xóa đc dữ liệu
                connection.Close();
            }
            return result;
        }

        public Employee? Get(int id)
        {
            Employee? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Employees where EmployeeID = @EmployeeID";
                var parameters = new
                {
                    EmployeeID = id
                };
                data = connection.QueryFirstOrDefault<Employee>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Orders where EmployeeID = @EmployeeID)
	                            select 1
                            else
	                            select 0";
                var parameters = new
                {
                    EmployeeID = id
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public List<Employee> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Employee> data = new List<Employee>();
            searchValue = $"%{searchValue}%"; //tìm kiếm tương đối với like

            using (var connection = OpenConnection())
            {
                var sql = @"select *
                          from	(
		                            select	*,
				                            row_number() over (order by FullName) as RowNumber
		                            from	Employees
		                            where	(FullName like @searchValue)
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
                data = connection.Query<Employee>(sql: sql, param: parameter, commandType: System.Data.CommandType.Text).ToList(); //truy vấn dữ liệu bằng cách cho connection truy vấn
                                                                                                                                   //dữ liệu về 1 Employee với câu lệnh sql là gì, tham số là gì và loại câu lệnh Text
                                                                                                                                   //(tính năng này là của Dapper giúp code nhanh hơn)
                connection.Close();
            }
            return data;
        }

        public bool Update(Employee data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Employees where EmployeeID <> @EmployeeID and Email = @Email)
                                begin
                                    update Employees
                                    set FullName = @FullName,
	                                    BirthDate  = @BirthDate,
	                                    Address = @Address,
	                                    Phone = @Phone,
	                                    Email = @Email,
                                        Photo = @Photo,
	                                    IsWorking = @IsWorking
                                    where EmployeeID = @EmployeeID
                                end";
                var parameters = new
                {
                    EmployeeID = data.EmployeeID,
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address,
                    Phone = data.Phone,
                    Email = data.Email,
                    Photo = data.Photo,
                    IsWorking = data.IsWorking
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0; //=0 ko có dòng nào đc cập nhật
                connection.Close();
            }
            return result;
        }
    }
}
