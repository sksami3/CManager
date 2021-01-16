using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKnowledge.Core.Entity.AuthenticationModels;

namespace Uknowledge.Business.Utilities
{
    public class Utilities
    {
        string con;
        public Utilities(string connectionString)
        {
            con = connectionString;
        }
        public async Task<List<string>> GetAllUserNamesByRole(string role)
        {
            List<string> usernameList;
            string sql = "select username username from AspNetUsers where id in (select UserId from AspNetUserRoles where RoleId = (select id from AspNetRoles where Name = '" + role + "'));";
            using (SqlConnection conn =
                    new SqlConnection(con))
            {
                await conn.OpenAsync();
                var usernameListAwait = await conn.QueryAsync<string>(sql);
                usernameList = usernameListAwait.ToList();
            }
            return usernameList;
        }
        
    }
}

