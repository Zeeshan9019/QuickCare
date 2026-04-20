using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using QuickCare.ViewModel;

namespace QuickCare.Repositories
{
    public class AccountRepository
    {
        private readonly IConfiguration _configuration;

        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection =>
            new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        public UserVM GetUserByUserName(string userName)
        {
            using var db = Connection;

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

            return db.QueryFirstOrDefault<UserVM>(query, new { UserName = userName });
        }
    }
}
