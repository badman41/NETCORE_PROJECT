using Dapper;
using Microsoft.Extensions.Configuration;
using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Transactions;

namespace OrderService.Infra.Data.Respository.Dapper
{
    public class InvoiceRepository : BaseRepository, IInvoiceRepository
    {
        public InvoiceRepository(IConfiguration config) : base (config)
        {
        }

        public InvoiceModel Add(Invoice entity)
        {
            InvoiceModel InvoiceModel;
            using (var transaction = new TransactionScope())
            {
                //thuc hien insert vao bang Invoice
                string sql = "INSERT INTO Invoice (Customerid,DeliveryTime,Note,TotalPrice,WeightTotal,Served,ServedTime,Status,CreatedAt,UpdatedAt,Code ) " +
                        "values (@Customerid,@deliveryTime,@note,@totalPrice,@weightTotal,@Served,@ServerTime,@Status,@CreatedAt,@UpdatedAt,@Code);" +
                        "SELECT * from Invoice where Id = CAST(SCOPE_IDENTITY() as int)";
                using (var connection = Connection)
                {
                    InvoiceModel = connection.QueryFirstOrDefault<InvoiceModel>(sql,
                        new
                        {
                            Customerid = (int)entity.Customer.Id.Value,
                            deliveryTime = entity.DeliveryTime.Value,
                            note = entity.Note.Value,
                            totalPrice = entity.TotalPrice.Value,
                            weightTotal = entity.WeightTotal.Value,
                            Served = false,
                            ServerTime = 300,
                            Status = 0,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            Code = entity.Code.Value
                        });
                    //thuc hien insert vao bang InvoiceItem
                    string sqlGetQuotaionItemId = "SELECT q.Id from QuotationItem q Join ProductUnit pu ON q.ProductUnitId = pu.Id" +
                            " JOIN Product p ON p.Id = pu.ProductId JOIN Unit u On u.Id = pu.UnitId" +
                            " Where QuotationId = @QuotationId AND p.Id = @productId AND u.Id = @unitId";
                    string sqlToInvoiceItem = "INSERT INTO InvoiceItem (InvoiceId,QuotationItemId,Quantity,Note,TotalPrice,Weight,Deliveried,DeliveriedQuantity ) " +
                                "values (@InvoiceId,@QuotationItemId,@quantity,@note,@totalPrice,@weight,@deliveried,@DeliveriedQuantity);";
                    //lay bao gia hien tai
                    int quotationId = connection.QueryFirstOrDefault<int>("Select Id From Quotation Where CustomerId = @customerid and EndDate is null",
                        new { customerid =(int) entity.Customer.Id.Value });
                    var affectedRows = 0;
                    if(quotationId > 0)
                    {
                        foreach (InvoiceItem item in entity.Items)
                        {

                            int quotationItemId = connection.QueryFirstOrDefault<int>(sqlGetQuotaionItemId,
                                new
                                {
                                    QuotationId = quotationId,
                                    productId = (int)item.Product.Id.Value,
                                    unitId = (int)item.Unit.Id.Value,
                                    CreatedAt = DateTime.Now

                                });
                            if(quotationItemId > 0)
                            {
                                affectedRows += connection.Execute(sqlToInvoiceItem,
                                    new
                                    {
                                        InvoiceId = InvoiceModel.ID,
                                        QuotationItemId = quotationItemId,
                                        quantity = item.Quantity.Value,
                                        note = item.Note.Value,
                                        totalPrice = item.TotalPrice.Value,
                                        weight = item.Weight.Value,
                                        deliveried = false,
                                        DeliveriedQuantity = 0,
                                        CreatedAt = DateTime.Now
                                    });
                            }
                            
                        }
                    }
                }
                
                transaction.Complete();
            } 
            return InvoiceModel;
        }

        public IEnumerable<InvoiceModel> All()
        {
            using (var cn = Connection)
            {
                var Customers = cn.Query<InvoiceModel>("SELECT Count(*) FROM Customer");
                return Customers;
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvoiceModel> Find(Expression<Func<InvoiceModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public InvoiceModel Get(int id)
        {
            string sqlSelect = "SELECT i.*, c.AddressId, c.Id as CustomerID, c.Code as CustomerCode, c.Name CustomerName" +
                                " FROM Invoice i join Customer c ON i.CustomerId = c.Id" +
                                " WHERE i.Id = @Id ";
            using (var connection = Connection)
            {
                var affectedRows = connection.QuerySingleOrDefault<InvoiceModel>(sqlSelect, new { Id = id});
                return affectedRows;
            }
        }

        public bool Update(Invoice entity)
        {
            int affectedRows;
            using (var transaction = new TransactionScope())
            {
                //thuc hien cap nhat vao bang Invoice
                string sql = "UPDATE Invoice" +
                        " SET Note = @Note," +
                        "    TotalPrice = @TotalPrice," +
                        "    WeightTotal = @WeightTotal," +
                        "    UpdatedAt = @UpdatedAt" +
                        " WHERE Id = @Id";

                using (var connection = Connection)
                {
                    affectedRows = 0;
                    affectedRows = connection.Execute(sql,
                        new
                        {
                            Id = (int)entity.Id.Value,
                            note = entity.Note.Value,
                            totalPrice = entity.TotalPrice.Value,
                            weightTotal = entity.WeightTotal.Value,
                            UpdatedAt = DateTime.Now
                        });
                    //thuc hien update bang InvoiceItem
                    string sqlToInvoiceItem = "INSERT INTO InvoiceItem (InvoiceId,QuotationItemId,Quantity,Note,TotalPrice,Weight,Deliveried,DeliveriedQuantity ) " +
                                "values (@InvoiceId,@QuotationItemId,@quantity,@note,@totalPrice,@weight,@deliveried,@DeliveriedQuantity);";
                    string sqlDeleteInvoiceItem = "DELETE InvoiceItem WHERE InvoiceId = @InvoiceId";
                    string sqlGetQuotaionItemId = "SELECT q.Id from QuotationItem q Join ProductUnit pu ON q.ProductUnitId = pu.Id" +
                            " JOIN Product p ON p.Id = pu.ProductId JOIN Unit u On u.Id = pu.UnitId" +
                            " Where QuotationId = @QuotationId AND p.Id = @productId AND u.Id = @unitId";
                    //lay bao gia hien tai
                    int quotationId = connection.QueryFirstOrDefault<int>("Select Id From Quotation Where CustomerId = @customerid and EndDate is null",
                        new { customerid = (int)entity.Customer.Id.Value });
                    //xoa item
                    affectedRows += connection.Execute(sqlDeleteInvoiceItem,
                        new
                        {
                            InvoiceId = (int)entity.Id.Value
                        });
                    //insert item moi
                    if (quotationId > 0)
                    {
                        foreach (InvoiceItem item in entity.Items)
                        {

                            int quotationItemId = connection.QueryFirstOrDefault<int>(sqlGetQuotaionItemId,
                                new
                                {
                                    QuotationId = quotationId,
                                    productId = (int)item.Product.Id.Value,
                                    unitId = (int)item.Unit.Id.Value,
                                    CreatedAt = DateTime.Now

                                });
                            if (quotationItemId > 0)
                            {
                                affectedRows += connection.Execute(sqlToInvoiceItem,
                                    new
                                    {
                                        InvoiceId = (int)entity.Id.Value,
                                        QuotationItemId = quotationItemId,
                                        quantity = item.Quantity.Value,
                                        note = item.Note.Value,
                                        totalPrice = item.TotalPrice.Value,
                                        weight = item.Weight.Value,
                                        deliveried = false,
                                        DeliveriedQuantity = 0,
                                        CreatedAt = DateTime.Now
                                    });
                            }

                        }
                    }
                }

                transaction.Complete();
            }
            return affectedRows > 0;
        }
        public PagedModel Search(DateTime? deliveryTime, string customerCode, int customerId, int page, int pageSize)
        {
            PagedModel model = new PagedModel();
            var dynamicParameters = new DynamicParameters();
            string sqlSelect = "SELECT i.*, c.AddressId, c.Id as CustomerID, c.Code as CustomerCode, c.Name CustomerName";
            string sqlCount = "SELECT COUNT(*)";
            string sqlFrom = " FROM Invoice i join Customer c ON i.CustomerId = c.Id";
            string sqlWhere = " WHERE 1 = 1 ";
            if (deliveryTime != null)
            {
                sqlWhere += " AND CONVERT(date, i.DeliveryTime) = CONVERT(date, @DeliveryTime)";
                dynamicParameters.Add("DeliveryTime", deliveryTime);
            }
            if (customerCode != null)
            {
                sqlWhere += " AND c.Code like @code";
                dynamicParameters.Add("code", "%" + customerCode + "%");
            }
            if (customerId > 0)
            {
                sqlWhere += " AND c.Id = @customerId";
                dynamicParameters.Add("customerId", customerId);
            }
            //lay total record
            using (var connection = Connection)
            {
                int total = connection.QuerySingle<int>(sqlCount + sqlFrom + sqlWhere, dynamicParameters);
                model.PageTotal = total;
                model.Page = page;
                model.PageSize = pageSize;
            }
            //search
            string sqlOrder = " order by i.DeliveryTime desc, c.Id";
            if (page != 0 && pageSize != 0)
            {
                sqlOrder += " OFFSET @page ROWS FETCH NEXT @pageSize ROWS ONLY";
            }

            dynamicParameters.Add("page", pageSize * (page - 1));
            dynamicParameters.Add("pageSize", pageSize);
            using (var connection = Connection)
            {
                var affectedRows = connection.Query<InvoiceModel>(sqlSelect + sqlFrom + sqlWhere + sqlOrder, dynamicParameters);
                model.Data = affectedRows;
            }
            return model;
        }
        public IEnumerable<ProductPriceModel> GetListProductOfInvoice(int InvoiceId)
        {
            string sql = "Select p.Id as ProductId, p.Code as ProductCode, p.Name as ProductName" +
                " from InvoiceItem qi join ProductUnit pu ON qi.ProductUnitId = pu.Id" +
                " Join Product p ON pu.ProductId = p.Id" +
                " Where qi.InvoiceId = @InvoiceId";
            using (var connection = Connection)
            {
                var rows = connection.Query<ProductPriceModel>(sql, new { InvoiceId = InvoiceId });
                return rows;
            }
        }

        public bool UpdateStatus(string invoiceCode, int status, bool served)
        {
            string sql = "UPDATE Invoice" +
                        " SET Status = @Status, Served = @Served" +
                        " WHERE Code = @Code";
            using (var connection = Connection)
            {
                var row = connection.Execute(sql, new { Status = status, Code = invoiceCode, Served = served });
                return row > 0;
            }
        }

        public string CancleStatus(int id)
        {
            string sql = "UPDATE Invoice" +
                        " SET Status = @Status" +
                        " WHERE Id = @Id;" +
                        " SELECT Code from Invoice Where Id = @Id";
            using (var connection = Connection)
            {
                var code = connection.QueryFirstOrDefault<string>(sql, new { Status = 3, Id = id});
                return code;
            }
        }
    }
}
