using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using QuickCare.ViewModel;

namespace QuickCare.Repositories
{
    public class AccountRepository
    {
        private readonly IDbConnection _conn;

        public AccountRepository(IDbConnection conn)
        {
            _conn = conn;
        }
        public UserVM GetUserByUserName(string userName)
        {

            string query = @"
            SELECT 
                U.UserId,
                U.UserName,
                U.PasswordHash,
                U.PhoneNumber,
                R.RoleName AS Role,
                U.RoleId AS RoleId
            FROM Membership.Users U
            INNER JOIN Membership.Roles R ON U.RoleId = R.RoleId
            WHERE U.UserName = @UserName";

            return _conn.QueryFirstOrDefault<UserVM>(query, new { UserName = userName });
        }
    }
}
