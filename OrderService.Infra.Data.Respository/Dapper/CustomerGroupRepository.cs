using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;
using OrderService.Domain.ReadModels;
using OrderService.Domain.Interface.Respository;
using System;
using System.Linq.Expressions;
using OrderService.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infra.Data.Respository.Dapper
{
    public class CustomerGroupRepository : BaseRepository, ICustomerGroupRepository
    {
        public CustomerGroupRepository(IConfiguration config) : base(config)
        {
        }

        public IEnumerable<CustomerGroupModel> Find(Expression<Func<CustomerGroupModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public CustomerGroupModel Get(int id)
        {
            string sql = "SELECT * FROM CustomerGroup Where Id =@Id;";

            using (var connection = Connection)
            {
                var affectedRows = connection.QuerySingleOrDefault<CustomerGroupModel>(sql,
                    new
                    {
                        Id = id
                    });
                return affectedRows;
            }
        }
        public IEnumerable<CustomerGroupModel> All()
        {
            using (var cn = Connection)
            {
                var CustomerGroups = cn.Query<CustomerGroupModel>("SELECT * FROM CustomerGroup");
                return CustomerGroups;
            }
        }

        public bool Delete(int id)
        {
            string sql = "DELETE CustomerGroup Where Id =@Id;";

            using (var connection = Connection)
            {
                var affectedRows = connection.Execute(sql,
                    new
                    {
                        Id = id
                    });
                return affectedRows > 0;
            }
        }

        public bool Update(CustomerGroup entity)
        {
            throw new NotImplementedException();
        }

        public void Add(CustomerGroup entity, int userId)
        {
            string sql = "INSERT INTO CustomerGroup (Name,Created_at,Created_by ) Values (@CustomerGroupName,@CreatedAt,@CreatedBy);";

            using (var connection = Connection)
            {
                var affectedRows = connection.Execute(sql,
                    new
                    {
                        CustomerGroupName = entity.Name,
                        CreatedAt = DateTime.Now,
                        CreatedBy = userId

                    });
            }
        }

        public CustomerGroupModel Add(CustomerGroup entity)
        {
            string sql = "INSERT INTO CustomerGroup (Name,CreatedAt,UpdatedAt ) Values (@CustomerGroupName,@CreatedAt,@UpdatedAt); SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = Connection)
            {
                var result = connection.Query<int>(sql,
                    new
                    {
                        CustomerGroupName = entity.Name.Value,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                if (result!=null )
                {
                    return new CustomerGroupModel((int)result.AsList().ToArray().GetValue(0), entity.Name.Value,  DateTime.Now, DateTime.Now);
                }
            }
            return null;
            
        }
    }
}
