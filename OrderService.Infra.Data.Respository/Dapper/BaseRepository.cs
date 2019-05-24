using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OrderService.Infra.Data.Respository.Dapper
{
    public class BaseRepository
    {
        private readonly IConfiguration _config;

        public BaseRepository(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("MyConnectionString"));
            }
        }
    }
}
