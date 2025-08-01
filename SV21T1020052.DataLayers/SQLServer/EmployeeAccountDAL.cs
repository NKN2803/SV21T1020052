using Dapper;
using SV21T1020052.DomainModels;

namespace SV21T1020052.DataLayers.SQLServer
{
    public class EmployeeAccountDAL : BaseDAL, IUserAccountDAL
    {
        public EmployeeAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string username, string password)
        {
            UserAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select EmployeeID as UserId,
		                    Email as UserName,
		                    FullName as DisplayName,
		                    Photo,
		                    RoleNames
                    from Employees
                    where Email = @Email and Password = @Password";
                var parameters = new
                {
                    Email = username,
                    Password = password
                };
                data = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            };
            return data;
        }

        public bool ChangePassword(string username, string oldpassword, string newpassword)
        {
            bool result = false;
            // so sánh mật khẩu cũ trong database
            UserAccount userAccount = Authorize(username, oldpassword);
            if (userAccount == null) return result;
            using (var connection = OpenConnection())
            {
                // cập nhật mật khẩu mới
                var sql = @"UPDATE  Employees
                    SET     Password = @Password
                    WHERE   Email = @Email";

                var parameters = new
                {
                    Email = username,
                    Password = newpassword
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Task<bool> ChangePasswordAsync(string username, string oldpassword, string newpassword)
        {
            throw new NotImplementedException();
        }


    }
}
