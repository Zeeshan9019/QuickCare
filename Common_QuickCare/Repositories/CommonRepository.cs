using Common_QuickCare.ViewModel;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Dapper;

namespace Common_QuickCare.Repositories
{
    public class CommonRepository
    {
        private readonly IConfiguration _configuration;
        public CommonRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private IDbConnection Connection =>
            new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        public CommonViewModel GetCommonInfo(int UserId)
        {
            using var db = Connection;

            string query = @"
            SELECT 
                U.UserId,
                U.UserName,
                U.PasswordHash,
                U.PhoneNumber,
                R.RoleName AS Role,
                U.RoleId AS RoleId,
	            org.*
            FROM Membership.Users U
            INNER JOIN Membership.Roles R ON U.RoleId = R.RoleId
            Inner Join Core.Organizations org on u.OrganizationId = org.OrganizationId
            WHERE U.UserId = @UserId";

            return db.QueryFirstOrDefault<CommonViewModel>(query, new { UserId });
        }
    }
}
