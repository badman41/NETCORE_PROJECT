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
    public class ProductRequestRepository : BaseRepository, IProductRequestRepository
    {
        public ProductRequestRepository(IConfiguration config) : base(config)
        {
        }

        public ProductRequestModel Add(ProductRequest entity)
        {
            ProductRequestModel ProductRequestModel;
            //thuc hien insert vao bang ProductRequest
            string sql = "INSERT INTO ProductRequest (ProductName,Description,Quantity,Status,CreatedAt,Response,UserId ) " +
                    "Values (@ProductName,@Description,@Quantity,@Status,@CreatedAt,@Response,@UserId);" +
                    "SELECT * from ProductRequest where Id = CAST(SCOPE_IDENTITY() as int)";
            using (var connection = Connection)
            {
                ProductRequestModel = connection.QueryFirstOrDefault<ProductRequestModel>(sql,
                    new
                    {
                        ProductName = entity.ProductName.Value,
                        Description = entity.Description.Value,
                        Quantity = entity.Quantity.Value,
                        Status = entity.Status.Value,
                        CreatedAt = DateTime.Now,
                        Response = entity.Response.Value,
                        UserId = (int)entity.Customer.Id.Value
                    });
            }
            return ProductRequestModel;
        }

        public IEnumerable<ProductRequestModel> All()
        {
            using (var cn = Connection)
            {
                var Customers = cn.Query<ProductRequestModel>("SELECT Count(*) FROM Customer");
                return Customers;
            }
        }

        public bool Delete(int id)
        {
            using (var cn = Connection)
            {
                var rows = cn.Execute("DELETE ProductRequest Where Id = @Id", new { Id = id});
                return rows > 0;
            }
        }

        public IEnumerable<ProductRequestModel> Find(Expression<Func<ProductRequestModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public ProductRequestModel Get(int id)
        {
            string sql = "SELECT c.*, p.UnitId, p.WeightPerUnit FROM ProductRequest as c JOIN ProductRequestUnit p ON c.Id = p.ProductRequestId WHERE p.IsCommonUnit = 1 AND c.Id = @Id";
            using (var connection = Connection)
            {
                var model = connection.QuerySingleOrDefault<ProductRequestModel>(sql, new { Id = id });
                return model;
            }
        }

        public bool Update(ProductRequest entity)
        {
            int affectedRows;
            //thuc hien update vao bang ProductRequest
            string sql = "Update ProductRequest " +
                    " Set Status = @Status," +
                    " Response = @Response" +
                    " Where Id = @Id";

            using (var connection = Connection)
            {
                affectedRows = connection.Execute(sql,
                    new
                    {
                        Status = entity.Status.Value,
                        Response = entity.Response.Value,
                        Id = (int)entity.Id.Value

                    });
            }
            return affectedRows > 0;
        }
        public PagedModel Search(int? status, int page, int pageSize)
        {
            PagedModel model = new PagedModel();
            var dynamicParameters = new DynamicParameters();
            string sqlSelect = "SELECT p.*, c.Name as CustomerName";
            string sqlCount = "SELECT COUNT(*)";
            string sqlFrom = " FROM ProductRequest as p JOIN Customer c ON p.UserId = c.Id";
            string sqlWhere = " WHERE 1 = 1 ";
            if (status != null)
            {
                sqlWhere += " AND p.Status = @status";
                dynamicParameters.Add("status", status);
            }
            using (var connection = Connection)
            {
                //lay total record
                int total = connection.QuerySingle<int>(sqlCount + sqlFrom + sqlWhere, dynamicParameters);
                model.PageTotal = total;
                model.Page = page;
                model.PageSize = pageSize;
                //search
                string sqlOrder = " order by p.CreatedAt DESC";
                if (page != 0 && pageSize != 0)
                {
                    sqlOrder += " OFFSET @page ROWS FETCH NEXT @pageSize ROWS ONLY";
                    dynamicParameters.Add("page", pageSize * (page - 1));
                    dynamicParameters.Add("pageSize", pageSize);
                }
                var affectedRows = connection.Query<ProductRequestModel>(sqlSelect + sqlFrom + sqlWhere + sqlOrder, dynamicParameters);
                model.Data = affectedRows;
            }
            return model;
        }
        public PagedModel GetAllByCustomer(int customerId, int page, int pageSize)
        {
            PagedModel model = new PagedModel();
            var dynamicParameters = new DynamicParameters();
            string sqlSelect = "SELECT p.*, c.Name as CustomerName";
            string sqlCount = "SELECT COUNT(*)";
            string sqlFrom = " FROM ProductRequest as p JOIN Customer c ON p.UserId = c.Id";
            string sqlWhere = " WHERE p.UserId = @userId ";
            dynamicParameters.Add("userId", customerId);
            using (var connection = Connection)
            {
                //lay total record
                int total = connection.QuerySingle<int>(sqlCount + sqlFrom + sqlWhere, dynamicParameters);
                model.PageTotal = total;
                model.Page = page;
                model.PageSize = pageSize;
                //search
                string sqlOrder = " order by p.CreatedAt DESC";
                if (page != 0 && pageSize != 0)
                {
                    sqlOrder += " OFFSET @page ROWS FETCH NEXT @pageSize ROWS ONLY";
                    dynamicParameters.Add("page", pageSize * (page - 1));
                    dynamicParameters.Add("pageSize", pageSize);
                }
                var affectedRows = connection.Query<ProductRequestModel>(sqlSelect + sqlFrom + sqlWhere + sqlOrder, dynamicParameters);
                model.Data = affectedRows;
            }
            return model;
        }
    }
}
