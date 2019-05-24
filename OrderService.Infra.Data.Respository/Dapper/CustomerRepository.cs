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
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(IConfiguration config) : base(config)
        {
        }

        public IEnumerable<CustomerModel> Find(Expression<Func<CustomerModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public CustomerModel Get(int id)
        {
            string sql = "SELECT * FROM Customer Where Id =@Id;";

            using (var connection = Connection)
            {
                var affectedRows = connection.QuerySingle<CustomerModel>(sql,
                    new
                    {
                        Id = id
                    });
                return affectedRows;
            }
        }
        public CustomerModel GetByCode(string code)
        {
            string sql = "SELECT * FROM Customer Where Code =@Code;";

            using (var connection = Connection)
            {
                var affectedRows = connection.QueryFirstOrDefault<CustomerModel>(sql,
                    new
                    {
                        Code = code
                    });
                return affectedRows;
            }
        }
        public IEnumerable<CustomerModel> All()
        {
            using (var cn = Connection)
            {
                var Customers = cn.Query<CustomerModel>("SELECT * FROM Customer");
                return Customers;
            }
        }

        public bool Delete(int id)
        {
            string sql = "DELETE Customer Where Id =@Id;";

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

        public bool Update(Customer entity)
        {
            string sql = "UPDATE Customer" +
                        " SET Name = @Name," +
                        " Note = @Note," +
                        " CartCode = @CartCode," +
                        " PhoneNumber = @PhoneNumber," +
                        " Email = @Email," +
                        " Code = @Code," +
                        " Status = @Status," +
                        " AddressId = @AddressId," +
                        " UpdatedAt = @UpdatedAt " +
                        " WHERE Id = @Id;";

            using (var connection = Connection)
            {
                var result = connection.Execute(sql,
                    new
                    {
                        Name = entity.Name.Value,
                        Note = entity.Note.Value,
                        CartCode = entity.CartCode.Value,
                        PhoneNumber = entity.PhoneNumber.Value,
                        Email = entity.Email.Value,
                        Code = entity.Code.Value,
                        Status = entity.Status.Value,
                        AddressId = (int)entity.Address.Id.Value,
                        UpdatedAt = DateTime.Now,
                        Id = (int) entity.Id.Value

                    });
                return result > 0;
            }
        }

        public void Add(Customer entity, int userId)
        {
            string sql = "INSERT INTO Customer (Name,Note,CartCode,PhoneNumber,Email,Code,Status,AddressId,CreatedAt,UpdatedAt ) " +
                        "Values (@Name,@Note,@CartCode,@PhoneNumber,@Email,@Code,@Status,@AddressId,@CreatedAt,@UpdatedAt);";

            using (var connection = Connection)
            {
                var affectedRows = connection.Execute(sql,
                    new
                    {
                        CustomerName = entity.Name,
                        CreatedAt = DateTime.Now,
                        CreatedBy = userId

                    });
            }
        }

        public CustomerModel Add(Customer entity)
        {
            string sql = "INSERT INTO Customer (Name,Note,CartCode,PhoneNumber,Email,Code,Status,AddressId,CreatedAt,UpdatedAt ) " +
                        "Values (@Name,@Note,@CartCode,@PhoneNumber,@Email,@Code,@Status,@AddressId,@CreatedAt,@UpdatedAt);"+
                        "SELECT * from Customer where Id = CAST(SCOPE_IDENTITY() as int)";

            using (var connection = Connection)
            {
                var result = connection.QueryFirstOrDefault<CustomerModel>(sql,
                    new
                    {
                        Name = entity.Name.Value,
                        Note = entity.Note.Value,
                        CartCode = entity.CartCode.Value,
                        PhoneNumber = entity.PhoneNumber.Value,
                        Email = entity.Email.Value,
                        Code = entity.Code.Value,
                        Status = entity.Status.Value,
                        AddressId = (int) entity.Address.Id.Value,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now

                    });
                return result;
            }
            
        }

        public PagedModel Search(CustomerModel customer, int page, int pageSize)
        {
            PagedModel model = new PagedModel();
            string sqlSelect = "SELECT *";
            string sqlCount = "SELECT COUNT(*)";
            string sqlFrom = " FROM Customer";
            string sqlWhere = " WHERE 1=1 ";
            var dynamicParameters = new DynamicParameters();
            if(customer.Name != null)
            {
                sqlWhere += " AND Name like @name";
                dynamicParameters.Add("name", "%" + customer.Name + "%");
            }
            if (customer.Code != null)
            {
                sqlWhere += " AND Code like @code";
                dynamicParameters.Add("code", "%" + customer.Code + "%");
            }
            if (customer.PhoneNumber != null)
            {
                sqlWhere += " AND PhoneNumber like @phoneNumber";
                dynamicParameters.Add("phoneNumber", "%" + customer.PhoneNumber + "%");
            }
            string sqlOrder = " order by Name";
            if(page !=0 && pageSize != 0)
            {
                sqlOrder += " OFFSET @page ROWS FETCH NEXT @pageSize ROWS ONLY";
                dynamicParameters.Add("page", pageSize * (page - 1));
                dynamicParameters.Add("pageSize", pageSize);
            }
            using (var connection = Connection)
            {
                //lay total record
                int total = connection.QuerySingle<int>(sqlCount + sqlFrom + sqlWhere, dynamicParameters);
                model.PageTotal = total;
                model.Page = page;
                model.PageSize = pageSize;
                //search
                var affectedRows = connection.Query<CustomerModel>(sqlSelect + sqlFrom + sqlWhere + sqlOrder, dynamicParameters);
                model.Data = affectedRows;
                return model;
            }
        }

        public IEnumerable<CustomerModel> GetListByCustomerGroup(int customerGroup)
        {
            string sql = "SELECT * FROM Customer Where CustomerGroupId =@CustomerGroupId;";

            using (var connection = Connection)
            {
                var result = connection.Query<CustomerModel>(sql,
                    new
                    {
                        CustomerGroupId = customerGroup
                    });
                return result;
            }
        }
        public bool ChangeCustomerGroup(int customerGroupId, int customerId)
        {
            string sql = "UPDATE Customer" +
                        " SET CustomerGroupId = @CustomerGroupId" +
                        " WHERE Id = @CustomerId;";

            using (var connection = Connection)
            {
                var result = connection.Execute(sql,
                    new
                    {
                        CustomerGroupId = customerGroupId,
                        CustomerId = customerId

                    });
                return result > 0;
            }
        }

        public IEnumerable<OrderedCustomerModel> GetListOrdered(DateTime date)
        {
            List<OrderedCustomerModel> models = new List<OrderedCustomerModel>();
            IEnumerable<CustomerModel> customers = All();
            
            string sql = "select Id from Invoice " +
                "where CAST(UpdatedAt AS DATE) = CAST(@Date AS DATE) and CustomerId = @CustomerId";

            using (var connection = Connection)
            {
                foreach (CustomerModel customer in customers)
                {
                    OrderedCustomerModel model = new OrderedCustomerModel()
                    {
                        CustomerID = customer.ID,
                        CustomerName = customer.Name,
                        Status = false
                    };
                    model.InvoiceIDs = connection.Query<int>(sql,
                    new
                    {
                        Date = date,
                        CustomerId = customer.ID
                    });
                    if(model.InvoiceIDs != null && model.InvoiceIDs.AsList().Count > 0)
                    {
                        model.Status = true;
                    }
                    models.Add(model);
                }
            }
            return models;
        }
    }
}
