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
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(IConfiguration config) : base (config)
        {
        }

        public AccountModel Add(AccountModel entity)
        {
            string sql = "INSERT INTO Account (DisplayName,Password,Role,Status,UserName,UpdatedAt,UserTypeID)"
                         + " values (@displayName,@password,@role,@status,@userName,@updatedAt,@userTypeID);"
                         + " SELECT * From Account Where Id = CAST(SCOPE_IDENTITY() as int)";

            using (var connection = Connection)
            {
                var result = connection.QuerySingleOrDefault<AccountModel>(sql,
                    new
                    {
                        displayName = entity.DisplayName,
                        password = entity.Password,
                        role = entity.Role,
                        status = entity.Status,
                        userName = entity.UserName,
                        updatedAt = entity.UpdatedAt,
                        userTypeID = entity.UserTypeID
                    });
                return result;
            }
        }

        public IEnumerable<AccountModel> All()
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AccountModel> Find(Expression<Func<AccountModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AccountModel get (string userName,string pass)
        {
            string sql = "Select * from Account where UserName = @UserName and Password = @Password";
            using (var connection = Connection)
            {
                AccountModel model = connection.QueryFirstOrDefault<AccountModel>(sql, new { UserName = userName, Password = pass });
                return model;
            }
        }
        public AccountModel getByCustomer(int userTypeId)
        {
            string sql = "Select * from Account where UserTypeId = @UserTypeId";
            using (var connection = Connection)
            {
                AccountModel model = connection.QueryFirstOrDefault<AccountModel>(sql, new { UserTypeId = userTypeId});
                return model;
            }
        }

        public AccountModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(AccountModel entity)
        {
            throw new NotImplementedException();
        }
        public PagedModel Search(AccountModel account, int page, int pageSize)
        {
            PagedModel model = new PagedModel();
            string sqlSelect = "SELECT *";
            string sqlCount = "SELECT COUNT(*)";
            string sqlFrom = " FROM Account";
            string sqlWhere = " WHERE 1=1 ";
            var dynamicParameters = new DynamicParameters();
            if (account.UserName != null)
            {
                sqlWhere += " AND UserName like @userName";
                dynamicParameters.Add("userName", "%" + account.UserName + "%");
            }
            if (account.Role != null)
            {
                sqlWhere += " AND Role = @role";
                dynamicParameters.Add("role", account.Role);
            }

            string sqlOrder = " order by UserName";
            if (page != 0 && pageSize != 0)
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
                var affectedRows = connection.Query<AccountModel>(sqlSelect + sqlFrom + sqlWhere + sqlOrder, dynamicParameters);
                model.Data = affectedRows;
                return model;
            }
        }

        public bool ChangePassword(AccountModel account, string oldPassword)
        {
            string sqlGet = " SELECT * From Account Where UserTypeId = @userTypeId AND Password = @password";
            string sqlUpdate = "UPDATE Account SET Password = @password WHERE Id = @Id";
            using (var connection = Connection)
            {
                var result = connection.QuerySingleOrDefault<AccountModel>(sqlGet,
                    new
                    {
                        password = oldPassword,
                        userTypeId = account.UserTypeID
                    });
                if (result == null) return false;
                connection.Execute(sqlUpdate,
                    new
                    {
                        password = account.Password,
                        Id = result.ID
                    });
                return true;
            }
        }
    }
}
