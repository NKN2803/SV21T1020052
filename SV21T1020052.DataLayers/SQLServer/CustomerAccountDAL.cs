using Dapper;
using SV21T1020052.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020052.DataLayers.SQLServer
{
    public class CustomerAccountDAL : BaseDAL, IUserAccountDAL
    {
        public CustomerAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string username, string password)
        {
            UserAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select CustomerID as UserId,
		                    Email as UserName,
		                    CustomerName as DisplayName,
		                    N'' as Photo,
		                    N'' as RoleNames
                    from Customers
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
            using (var connection = OpenConnection())
            {
                var sql = @"
            UPDATE Customers
            SET Password = @NewPassword
            WHERE Email = @Username AND Password = @OldPassword";

                var parameters = new
                {
                    Username = username,
                    OldPassword = oldpassword,
                    NewPassword = newpassword
                };

                int rowsAffected = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                return rowsAffected > 0;
            }
        }

        public Customer? GetUserProfile(string username)
        {
            throw new NotImplementedException();
        }

        public bool RegisterUser(UserAccount newUser)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCustomerProfile(Customer customer)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUserProfile(string username, string displayName, string email, string phone, string provice, string address)
        {
            throw new NotImplementedException();
        }
    }
}