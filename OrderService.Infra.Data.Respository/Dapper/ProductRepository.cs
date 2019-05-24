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
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(IConfiguration config) : base (config)
        {
        }

        public ProductModel Add(Product entity)
        {
            ProductModel productModel;
            using (var transaction = new TransactionScope())
            {
                //thuc hien insert vao bang product
                string sql = "INSERT INTO Product (Name,Note,Code,PreservationId,CreatedAt,UpdatedAt ) " +
                        "Values (@Name,@Note,@Code,@PreservationId,@CreatedAt,@UpdatedAt);" +
                        "SELECT * from Product where Id = CAST(SCOPE_IDENTITY() as int)";
                using (var connection = Connection)
                {
                    productModel = connection.QueryFirstOrDefault<ProductModel>(sql,
                        new
                        {
                            Name = entity.Name.Value,
                            Note = entity.Note.Value,
                            Code = entity.Code.Value,
                            PreservationId = (int) entity.Preservation.Id.Value,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now

                        });
                }
                //thuc hien insert vao bang ProductUnit
                string sqlToProduct = "INSERT INTO ProductUnit (ProductId,UnitId,IsCommonUnit,WeightPerUnit,CreatedAt ) " +
                            "Values (@ProductId,@UnitId,@IsCommonUnit,@WeightPerUnit,@CreatedAt);";

                using (var connection = Connection)
                {
                    var affectedRows = connection.Execute(sqlToProduct,
                        new
                        {
                            ProductId = productModel.Id,
                            UnitId = (int)entity.CommonUnit.Id.Value,
                            IsCommonUnit = 1,
                            WeightPerUnit = entity.WeightPerUnit.Value,
                            CreatedAt = DateTime.Now

                        });
                }
                foreach (KeyValuePair<Unit, decimal> entry in entity.OtherUnitOfProduct)
                {
                    string sqlOtherUnit = "INSERT INTO ProductUnit (ProductId,UnitId,IsCommonUnit,WeightPerUnit,CreatedAt ) " +
                            "Values (@ProductId,@UnitId,@IsCommonUnit,@WeightPerUnit,@CreatedAt);";

                    using (var connection = Connection)
                    {
                        var affectedRows = connection.Execute(sqlOtherUnit,
                            new
                            {
                                ProductId = productModel.Id,
                                UnitId = (int)entry.Key.Id.Value,
                                IsCommonUnit = 0,
                                WeightPerUnit = entry.Value,
                                CreatedAt = DateTime.Now

                            });
                    }
                }
                transaction.Complete();
            } 
            return productModel;
        }

        public IEnumerable<ProductModel> All()
        {
            using (var cn = Connection)
            {
                var Customers = cn.Query<ProductModel>("SELECT Count(*) FROM Customer");
                return Customers;
            }
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductModel> Find(Expression<Func<ProductModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public ProductModel Get(int id)
        {
            string sql = "SELECT c.*, p.UnitId, p.WeightPerUnit FROM Product as c JOIN ProductUnit p ON c.Id = p.ProductId WHERE p.IsCommonUnit = 1 AND c.Id = @Id";
            using (var connection = Connection)
            {
                var model = connection.QuerySingleOrDefault<ProductModel>(sql, new { Id = id});
                return model;
            }
        }

        public bool Update(Product entity)
        {
            int affectedRows;
            using (var transaction = new TransactionScope())
            {
                //thuc hien update vao bang product
                string sql = "Update Product " +
                        " Set Name = @Name," +
                        " Note = @Note," +
                        " PreservationId = @PreservationId," +
                        " Code = @Code," +
                        " UpdatedAt = @UpdatedAt" +
                        " Where Id = @Id";


                using (var connection = Connection)
                {
                    affectedRows = connection.Execute(sql,
                        new
                        {
                            Name = entity.Name.Value,
                            Note = entity.Note.Value,
                            Code = entity.Code.Value,
                            PreservationId = (int) entity.Preservation.Id.Value,
                            UpdatedAt = DateTime.Now,
                            Id = (int)entity.Id.Value

                        });
                    IEnumerable<int> ProductUnitIds = connection.Query<int>(" Select id from ProductUnit Where ProductId = @Id;", new { Id = (int)entity.Id.Value });
                    affectedRows = connection.Execute("Delete ProductUnit Where ProductId = @Id;", new { Id = (int)entity.Id.Value });
                    //sua lai thong tin bao gia
                    affectedRows = connection.Execute("Delete QuotationItem Where ProductUnitId IN @Ids", new { Ids = ProductUnitIds });
                    //thuc hien insert vao bang ProductUnit
                    string sqlToProduct = "INSERT INTO ProductUnit (ProductId,UnitId,IsCommonUnit,WeightPerUnit,CreatedAt ) " +
                                "Values (@ProductId,@UnitId,@IsCommonUnit,@WeightPerUnit,@CreatedAt);";

                    affectedRows = connection.Execute(sqlToProduct,
                        new
                        {
                            ProductId = (int)entity.Id.Value,
                            UnitId = (int)entity.CommonUnit.Id.Value,
                            IsCommonUnit = 1,
                            WeightPerUnit = entity.WeightPerUnit.Value,
                            CreatedAt = DateTime.Now

                        });
                    foreach (KeyValuePair<Unit, decimal> entry in entity.OtherUnitOfProduct)
                    {
                        string sqlOtherUnit = "INSERT INTO ProductUnit (ProductId,UnitId,IsCommonUnit,WeightPerUnit,CreatedAt ) " +
                                "Values (@ProductId,@UnitId,@IsCommonUnit,@WeightPerUnit,@CreatedAt);";

                        affectedRows = connection.Execute(sqlOtherUnit,
                            new
                            {
                                ProductId = (int)entity.Id.Value,
                                UnitId = (int)entry.Key.Id.Value,
                                IsCommonUnit = 0,
                                WeightPerUnit = entry.Value,
                                CreatedAt = DateTime.Now

                            });
                    }
                }
                transaction.Complete();
            }
            return affectedRows > 0;
        }
        public PagedModel Search(ProductModel product, int page, int pageSize)
        {
            PagedModel model = new PagedModel();
            var dynamicParameters = new DynamicParameters();
            string sqlSelect = "SELECT c.*, p.UnitId, p.WeightPerUnit";
            string sqlCount = "SELECT COUNT(*)";
            string sqlFrom = " FROM Product as c JOIN ProductUnit p ON c.Id = p.ProductId";
            string sqlWhere = " WHERE p.IsCommonUnit = 1 ";
            if (product.Name != null)
            {
                sqlWhere += " AND c.Name like @name";
                dynamicParameters.Add("name", "%" + product.Name + "%");
            }
            if (product.Code != null)
            {
                sqlWhere += " AND c.Code like @code";
                dynamicParameters.Add("code", "%" + product.Code + "%");
            }
            if (product.PreservationId > 0)
            {
                sqlWhere += " AND c.PreservationId = @preservationId";
                dynamicParameters.Add("preservationId",product.PreservationId);
            }
            using (var connection = Connection)
            {
                //lay total record
                int total = connection.QuerySingle<int>(sqlCount + sqlFrom + sqlWhere, dynamicParameters);
                model.PageTotal = total;
                model.Page = page;
                model.PageSize = pageSize;
                //search
                string sqlOrder = "";
                if (page != 0 && pageSize != 0)
                {
                    sqlOrder = " order by c.Id OFFSET @page ROWS FETCH NEXT @pageSize ROWS ONLY";
                }
            
                dynamicParameters.Add("page", pageSize * (page - 1));
                dynamicParameters.Add("pageSize", pageSize);
                var affectedRows = connection.Query<ProductModel>(sqlSelect + sqlFrom + sqlWhere + sqlOrder, dynamicParameters);
                model.Data = affectedRows;
            }
            return model;
        }

        public PagedModel GetAllProductByCustomer(int customerId, int page, int pageSize)
        {
            PagedModel model = new PagedModel();
            var dynamicParameters = new DynamicParameters();
            string sqlSelect = "SELECT c.*, p.UnitId, p.WeightPerUnit";
            string sqlCount = "SELECT COUNT(*)";
            string sqlFrom = " FROM Product as c JOIN ProductUnit p ON c.Id = p.ProductId" +
                " join QuotationItem qi on qi.ProductUnitId = p.Id" +
                " join Quotation q on q.Id = qi.QuotationId";
            string sqlWhere = " WHERE p.IsCommonUnit = 1 AND q.CustomerId = @CustomerId AND q.EndDate is NULL";
            dynamicParameters.Add("CustomerId", customerId);
            
            using (var connection = Connection)
            {
                //lay total record
                int total = connection.QuerySingle<int>(sqlCount + sqlFrom + sqlWhere, dynamicParameters);
                model.PageTotal = total;
                model.Page = page;
                model.PageSize = pageSize;
                //search
                string sqlOrder = "";
                if (page != 0 && pageSize != 0)
                {
                    sqlOrder = " order by c.Id OFFSET @page ROWS FETCH NEXT @pageSize ROWS ONLY";
                }

                dynamicParameters.Add("page", 1);
                dynamicParameters.Add("pageSize", 10000);
                var affectedRows = connection.Query<ProductModel>(sqlSelect + sqlFrom + sqlWhere + sqlOrder, dynamicParameters);
                model.Data = affectedRows;
            }
            return model;
        }

        public decimal GetWeightPerUnit(int productId, int unitId)
        {
            string sql = "SELECT pu.WeightPerUnit from ProductUnit pu" +
                            " JOIN Product p ON p.Id = pu.ProductId JOIN Unit u On u.Id = pu.UnitId" +
                            " Where p.Id = @ProductId AND u.Id = @UnitId";
            using (var connection = Connection)
            {
                decimal Weight = connection.QueryFirstOrDefault<decimal>(sql, new { ProductId = productId, UnitId = unitId });
                return Weight;
            }
        }
        public ProductModel GetByCode(string code)
        {
            string sql = "SELECT * FROM Product Where Code =@Code;";

            using (var connection = Connection)
            {
                var affectedRows = connection.QueryFirstOrDefault<ProductModel>(sql,
                    new
                    {
                        Code = code
                    });
                return affectedRows;
            }
        }
    }
}
