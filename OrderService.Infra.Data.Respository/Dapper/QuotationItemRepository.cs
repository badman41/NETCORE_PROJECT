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
    public class QuotationItemRepository : BaseRepository, IQuotationItemRepository
    {
        public QuotationItemRepository(IConfiguration config) : base(config)
        {
        }

        public IEnumerable<QuotationItemModel> Find(Expression<Func<QuotationItemModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public QuotationItemModel Get(int id)
        {
            string sql = "SELECT * FROM QuotationItem Where Id =@Id;";

            using (var connection = Connection)
            {
                var affectedRows = connection.QuerySingle<QuotationItemModel>(sql,
                    new
                    {
                        Id = id
                    });
                return affectedRows;
            }
        }
        public IEnumerable<QuotationItemModel> All()
        {
            using (var cn = Connection)
            {
                var QuotationItems = cn.Query<QuotationItemModel>("SELECT * FROM QuotationItem");
                return QuotationItems;
            }
        }

        public bool Delete(int id)
        {
            string sql = "DELETE QuotationItem Where Id =@Id;";

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

        public bool Update(QuotationItem entity)
        {
            int productUnitId = GetProductUnitId((int)entity.Unit.Id.Value, entity.Product.Code.Value);

            if (productUnitId > 0)
            {
                string sqlUpdate = "Update QuotationItem " +
                    " SET Price = @Price," +
                    " UpdatedAt = @UpdatedAt" +
                    " WHERE QuotationId = @QuotationId and ProductUnitId = @ProductUnitId";
                using (var connection = Connection)
                {
                    var rows = connection.Execute(sqlUpdate,
                        new
                        {
                            QuotationId = (int)entity.Quotation.Id.Value,
                            ProductUnitId = productUnitId,
                            Price = entity.Price.Value,
                            UpdatedAt = DateTime.Now
                        });
                    return (rows > 0);
                }
            }
            return false;
        }

        public void Add(QuotationItem entity, int userId)
        {
        }

        public QuotationItemModel Add(QuotationItem entity)
        {
            int productUnitId = GetProductUnitId ((int)entity.Unit.Id.Value, entity.Product.Code.Value);
            
            if(productUnitId > 0)
            {
                string sqlInsert = "INSERT INTO QuotationItem (QuotationId,ProductUnitId,Price,CreatedAt,UpdatedAt)"
                        + " Values (@QuotationId,@ProductUnitId,@Price,@CreatedAt,@UpdatedAt);"
                        + " SELECT * From QuotationItem Where Id = CAST(SCOPE_IDENTITY() as int)";
                using (var connection = Connection)
                {
                    var result = connection.QuerySingleOrDefault<QuotationItemModel>(sqlInsert,
                        new
                        {
                            QuotationId = (int) entity.Quotation.Id.Value,
                            ProductUnitId = productUnitId,
                            Price = entity.Price.Value,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        });
                    return result;
                }
            }
            return null;
        }

        public IEnumerable<QuotationItemModel> GetListItemOfQuotation(int quotationId, int productId)
        {
            string sql = "select qi.Price, pu.UnitId, u.Name as UnitName" +
                " from ProductUnit pu join  Unit u on pu.UnitId=u.Id" +
                " join QuotationItem qi on pu.Id = qi.ProductUnitId" +
                " Where qi.QuotationId  =@QuotationId and pu.ProductId = @ProductId";
            using (var cn = Connection)
            {
                var QuotationItems = cn.Query<QuotationItemModel>(sql, new { QuotationId  = quotationId, @ProductId  = productId });
                return QuotationItems;
            }
        }
        public QuotationItemModel GetByProperties(QuotationItem entity)
        {
            string sql = "SELECT * FROM QuotationItem q" +
                " JOIN ProductUnit pu ON q.ProductUnitId = pu.Id" +
                " JOIN Product p On pu.ProductId = p.Id" +
                " Where pu.UnitId =@UnitId and p.Code = @ProductCode and q.QuotationId = @QuotationId;";

            using (var connection = Connection)
            {
                var model = connection.QueryFirstOrDefault<QuotationItemModel>(sql,
                    new
                    {
                        UnitId = (int) entity.Unit.Id.Value,
                        ProductCode = entity.Product.Code.Value,
                        QuotationId = (int) entity.Quotation.Id.Value
                    });
                return model;
            }
        }
        public int GetProductUnitId(int unitId,string productCode)
        {
            string sqlGet = "SELECT pu.Id FROM ProductUnit pu join Product p On pu.ProductId = p.Id Where pu.UnitId =@UnitId and p.Code = @Code;";
            using (var connection = Connection)
            {
                int productUnitId = connection.QueryFirstOrDefault<int>(sqlGet,
                    new
                    {
                        UnitId = unitId,
                        Code = productCode
                    });
                return productUnitId;
            }
        }
    }
}
