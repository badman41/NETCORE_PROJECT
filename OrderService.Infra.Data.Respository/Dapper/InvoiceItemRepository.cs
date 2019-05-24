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
    public class InvoiceItemRepository : BaseRepository, IInvoiceItemRepository
    {
        public InvoiceItemRepository(IConfiguration config) : base(config)
        {
        }

        public IEnumerable<InvoiceItemModel> Find(Expression<Func<InvoiceItemModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public InvoiceItemModel Get(int id)
        {
            string sql = "SELECT * FROM InvoiceItem Where Id =@Id;";

            using (var connection = Connection)
            {
                var affectedRows = connection.QuerySingle<InvoiceItemModel>(sql,
                    new
                    {
                        Id = id
                    });
                return affectedRows;
            }
        }
        public IEnumerable<InvoiceItemModel> All()
        {
            using (var cn = Connection)
            {
                var InvoiceItems = cn.Query<InvoiceItemModel>("SELECT * FROM InvoiceItem");
                return InvoiceItems;
            }
        }

        public bool Delete(int id)
        {
            string sql = "DELETE InvoiceItem Where Id =@Id;";

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

        public bool Update(InvoiceItem entity)
        {
            int productUnitId = 1;

            if (productUnitId > 0)
            {
                string sqlUpdate = "Update InvoiceItem " +
                    " SET Price = @Price," +
                    " UpdatedAt = @UpdatedAt" +
                    " WHERE InvoiceId = @InvoiceId and ProductUnitId = @ProductUnitId";
                using (var connection = Connection)
                {
                    var rows = connection.Execute(sqlUpdate,
                        new
                        {
                            //InvoiceId = (int)entity.Invoice.Id.request,
                            ProductUnitId = productUnitId,
                            Price = entity.Price.Value,
                            UpdatedAt = DateTime.Now
                        });
                    return (rows > 0);
                }
            }
            return false;
        }

        public void Add(InvoiceItem entity, int userId)
        {
        }

        public InvoiceItemModel Add(InvoiceItem entity)
        {
            int productUnitId = 1;
            
            if(productUnitId > 0)
            {
                string sqlInsert = "INSERT INTO InvoiceItem (InvoiceId,ProductUnitId,Price,CreatedAt,UpdatedAt)"
                        + " values (@InvoiceId,@ProductUnitId,@Price,@CreatedAt,@UpdatedAt);"
                        + " SELECT * From InvoiceItem Where Id = CAST(SCOPE_IDENTITY() as int)";
                using (var connection = Connection)
                {
                    var result = connection.QuerySingleOrDefault<InvoiceItemModel>(sqlInsert,
                        new
                        {
                            //InvoiceId = (int) entity.Invoice.Id.request,
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

        public IEnumerable<InvoiceItemModel> GetListItemOfInvoice(int invoiceId)
        {
            string sql = "SELECT it.*, qi.Price, p.Id as ProductID, p.Name as ProductName, u.Id as UnitID, u.Name as UnitName" +
                " FROM InvoiceItem it join QuotationItem qi on it.QuotationItemId = qi.Id" +
                " join ProductUnit pu on qi.ProductUnitId = pu.Id" +
                " join Product p on pu.ProductId = p.Id" +
                " join Unit u on pu.UnitId = u.Id" +
                " Where it.InvoiceId = @InvoiceId";
            using (var cn = Connection)
            {
                var InvoiceItems = cn.Query<InvoiceItemModel>(sql, new { InvoiceId = invoiceId });
                return InvoiceItems;
            }
        }
    }
}
