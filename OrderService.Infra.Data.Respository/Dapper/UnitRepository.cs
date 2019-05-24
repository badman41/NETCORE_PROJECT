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
    public class UnitRepository : BaseRepository, IUnitRepository
    {
        public UnitRepository(IConfiguration config) : base(config)
        {
        }

        public IEnumerable<UnitModel> Find(Expression<Func<UnitModel, bool>> predicate)
        {
            using (var cn = Connection)
            {
                //var albuns = null;
                return null;
            }
        }

        public UnitModel Get(int id)
        {
            string sql = "SELECT * FROM Unit Where Id =@Id;";

            using (var connection = Connection)
            {
                var affectedRows = connection.QuerySingle<UnitModel>(sql,
                    new
                    {
                        Id = id
                    });
                return affectedRows;
            }
        }
        public UnitModel GetByName(string name)
        {
            string sql = "SELECT * FROM Unit Where LOWER(Name) =@Name;";

            using (var connection = Connection)
            {
                var affectedRows = connection.QueryFirstOrDefault<UnitModel>(sql,
                    new
                    {
                        Name = name.ToLower()
                    });
                return affectedRows;
            }
        }
        public IEnumerable<UnitModel> All()
        {
            using (var cn = Connection)
            {
                var units = cn.Query<UnitModel>("SELECT * FROM Unit");
                return units;
            }
        }

        public bool Delete(int id)
        {
            string sql = "DELETE Unit Where Id =@Id;";

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

        public bool Update(Unit entity)
        {
            throw new NotImplementedException();
        }

        public void Add(Unit entity, int userId)
        {
            string sql = "INSERT INTO Unit (Name,Created_at,Created_by ) Values (@UnitName,@CreatedAt,@CreatedBy);";

            using (var connection = Connection)
            {
                var affectedRows = connection.Execute(sql,
                    new
                    {
                        UnitName = entity.Name,
                        CreatedAt = DateTime.Now,
                        CreatedBy = userId

                    });
            }
        }

        public UnitModel Add(Unit entity)
        {
            string sql = "INSERT INTO Unit (Name,CreatedAt ) Values (@UnitName,@CreatedAt); SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = Connection)
            {
                var result = connection.Query<int>(sql,
                    new
                    {
                        UnitName = entity.Name.Value,
                        CreatedAt = DateTime.Now

                    });
                if (result!=null )
                {
                    return new UnitModel((int)result.AsList().ToArray().GetValue(0), entity.Name.Value, null, DateTime.Now,null,null);
                }
            }
            return null;
            
        }

        public IEnumerable<ProductUnitModel> GetALlProductUnitOfCustomer(int productId,int customerId)
        {
            string sql = "SELECT p.Id,p.ProductId,p.UnitId,p.WeightPerUnit as WPU,qi.Price, Unit.Name " +
                "FROM ProductUnit as p Join Unit  on p.UnitId = Unit.Id " +
                " JOIN QuotationItem qi On p.Id = qi.ProductUnitId " +
                " JOIN Quotation q On q.Id = qi.QuotationId" +
                " Where p.ProductId =@Id AND q.CustomerId = @CustomerId;";

            using (var connection = Connection)
            {
                var affectedRows = connection.Query<ProductUnitModel>(sql,
                    new
                    {
                        Id = productId,
                        CustomerId = customerId
                    });
                return affectedRows;
            }
        }
        public IEnumerable<ProductUnitModel> GetALlOtherUnitOfProduct(int productId)
        {
            string sql = "SELECT p.UnitId as ID,p.ProductId,p.UnitId,p.WeightPerUnit as WPU, Unit.Name " +
                " FROM ProductUnit as p Join Unit  on p.UnitId = Unit.Id " +
                " Where p.ProductId =@Id";

            using (var connection = Connection)
            {
                var affectedRows = connection.Query<ProductUnitModel>(sql,
                    new
                    {
                        Id = productId
                    });
                return affectedRows;
            }
        }
    }
}
