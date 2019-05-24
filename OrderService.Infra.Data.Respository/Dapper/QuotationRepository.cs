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
    public class QuotationRepository : BaseRepository, IQuotationRepository
    {
        public QuotationRepository(IConfiguration config) : base (config)
        {
        }

        public QuotationModel Add(Quotation entity)
        {
            QuotationModel QuotationModel;
            using (var transaction = new TransactionScope())
            {
                //thuc hien ket thuc qua trinh truoc
                string sqlEndProccess = "UPDATE Quotation SET EndDate = @EndDate" +
                    " WHERE EndDate is NULL AND CustomerId = @CustomerId";
                //thuc hien insert vao bang Quotation
                string sql = "INSERT INTO Quotation (CustomerID,StartDate,CreatedAt,UpdatedAt ) " +
                        "Values (@CustomerID,@StartDate,@CreatedAt,@UpdatedAt);" +
                        "SELECT *,StartDate as Date from Quotation where Id = CAST(SCOPE_IDENTITY() as int)";
                using (var connection = Connection)
                {
                    var result = connection.Execute(sqlEndProccess,
                        new
                        {
                            EndDate = entity.StartDate.Value.AddDays(-1),
                            CustomerId = (int)entity.Customer.Id.Value
                        });
                    QuotationModel = connection.QueryFirstOrDefault<QuotationModel>(sql,
                        new
                        {
                            CustomerID = (int)entity.Customer.Id.Value,
                            StartDate = entity.StartDate.Value,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now

                        });
                }
                transaction.Complete();
            } 
            return QuotationModel;
        }

        public IEnumerable<QuotationModel> All()
        {
            using (var cn = Connection)
            {
                var Customers = cn.Query<QuotationModel>("SELECT Count(*) FROM Customer");
                return Customers;
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuotationModel> Find(Expression<Func<QuotationModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public QuotationModel Get(int id)
        {
            string sql = "SELECT c.*,c.StartDate as Date, p.UnitId, p.WeightPerUnit FROM Quotation as c JOIN QuotationUnit p ON c.Id = p.QuotationId WHERE p.IsCommonUnit = 1 AND c.Id = @Id";
            using (var connection = Connection)
            {
                var model = connection.QuerySingleOrDefault<QuotationModel>(sql, new { Id = id});
                return model;
            }
        }

        public bool Update(Quotation entity)
        {
            int affectedRows;
            using (var transaction = new TransactionScope())
            {
                //thuc hien update vao bang Quotation
                string sql = "Update Quotation " +
                        " Set Name = @Name," +
                        " Note = @Note," +
                        " Code = @Code," +
                        " UpdatedAt = @UpdatedAt" +
                        " Where Id = @Id";

                
                using (var connection = Connection)
                {
                    affectedRows = connection.Execute(sql,
                        new
                        {
                            //Name = entity.Name.Value,
                            //Note = entity.Note.Value,
                            //Code = entity.Code.Value,
                            UpdatedAt = DateTime.Now,
                            Id = (int)entity.Id.Value

                        });
                }
                
                //thuc hien xoa thong tin bang QuotationUnit
                using (var connection = Connection)
                {
                    affectedRows = connection.Execute("Delete QuotationUnit Where QuotationId = @Id", new { Id = (int)entity.Id.Value });
                }
                //thuc hien insert vao bang QuotationUnit
                string sqlToQuotation = "INSERT INTO QuotationUnit (QuotationId,UnitId,IsCommonUnit,WeightPerUnit,CreatedAt ) " +
                            "Values (@QuotationId,@UnitId,@IsCommonUnit,@WeightPerUnit,@CreatedAt);";

                using (var connection = Connection)
                {
                    affectedRows = connection.Execute(sqlToQuotation,
                        new
                        {
                            QuotationId = (int)entity.Id.Value,
                            //UnitId = (int)entity.CommonUnit.Id.Value,
                            //IsCommonUnit = 1,
                            //WeightPerUnit = entity.WeightPerUnit.Value,
                            CreatedAt = DateTime.Now

                        });
                }
                //foreach (KeyValuePair<Unit, decimal> entry in entity.OtherUnitOfQuotation)
                //{
                //    string sqlOtherUnit = "INSERT INTO QuotationUnit (QuotationId,UnitId,IsCommonUnit,WeightPerUnit,CreatedAt ) " +
                //            "Values (@QuotationId,@UnitId,@IsCommonUnit,@WeightPerUnit,@CreatedAt);";

                //    using (var connection = Connection)
                //    {
                //        affectedRows = connection.Execute(sqlOtherUnit,
                //            new
                //            {
                //                QuotationId = (int)entity.Id.Value,
                //                UnitId = (int)entry.Key.Id.Value,
                //                IsCommonUnit = 0,
                //                WeightPerUnit = entry.Value,
                //                CreatedAt = DateTime.Now

                //            });
                //    }
                //}
                transaction.Complete();
            }
            return affectedRows > 0;
        }
        public PagedModel Search(string customerCode, int page, int pageSize)
        {
            PagedModel model = new PagedModel();
            var dynamicParameters = new DynamicParameters();
            string sqlSelect = "select q.*,q.StartDate as Date, c.Code as CustomerCode";
            string sqlCount = "SELECT COUNT(*)";
            string sqlFrom = " FROM Quotation q join Customer c On q.CustomerId = c.Id";
            string sqlWhere = " WHERE c.Code = @CustomerCode ";
            
            //lay total record
            using (var connection = Connection)
            {
                int total = connection.QuerySingle<int>(sqlCount + sqlFrom + sqlWhere, new { CustomerCode = customerCode });
                model.PageTotal = total;
                model.Page = page;
                model.PageSize = pageSize;
                //search
                string sqlOrder = " order by q.StartDate desc OFFSET @page ROWS FETCH NEXT @pageSize ROWS ONLY";
                IEnumerable<QuotationModel> affectedRows;
                if (page == 0 && pageSize == 0)
                {
                    affectedRows = connection.Query<QuotationModel>(sqlSelect + sqlFrom + sqlWhere,
                        new { CustomerCode = customerCode});
                }
                else
                {
                    affectedRows = connection.Query<QuotationModel>(sqlSelect + sqlFrom + sqlWhere + sqlOrder,
                    new { CustomerCode = customerCode, Page = pageSize * (page - 1), PageSize = pageSize });
                }
                
                model.Data = affectedRows;
            }
            return model;
        }
        public IEnumerable<ProductPriceModel> GetListProductOfQuotation(int quotationId)
        {
            string sql = "Select p.Id as ProductId, p.Code as ProductCode, p.Name as ProductName" +
                " from QuotationItem qi join ProductUnit pu ON qi.ProductUnitId = pu.Id" +
                " Join Product p ON pu.ProductId = p.Id" +
                " Where qi.QuotationId = @QuotationId";
            using (var connection = Connection)
            {
                var rows = connection.Query<ProductPriceModel>(sql, new { QuotationId = quotationId });
                return rows;
            }
        }
    }
}
