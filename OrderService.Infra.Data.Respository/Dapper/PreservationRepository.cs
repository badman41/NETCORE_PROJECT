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
    public class PreservationRepository : BaseRepository, IPreservationRepository
    {
        public PreservationRepository(IConfiguration config) : base(config)
        {
        }

        public IEnumerable<PreservationModel> Find(Expression<Func<PreservationModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public PreservationModel Get(int id)
        {
            string sql = "SELECT * FROM Preservation Where Id =@Id;";

            using (var connection = Connection)
            {
                var affectedRows = connection.QuerySingleOrDefault<PreservationModel>(sql,
                    new
                    {
                        Id = id
                    });
                return affectedRows;
            }
        }
        public IEnumerable<PreservationModel> All()
        {
            using (var cn = Connection)
            {
                var Preservations = cn.Query<PreservationModel>("SELECT * FROM Preservation");
                return Preservations;
            }
        }

        public bool Delete(int id)
        {
            string sql = " UPDATE Product SET PreservationId = NULL WHERE PreservationId = @Id;" +
                " DELETE Preservation Where Id =@Id;";

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

        public bool Update(Preservation entity)
        {
            throw new NotImplementedException();
        }

        public void Add(Preservation entity, int userId)
        {
        }

        public PreservationModel Add(Preservation entity)
        {
            string sql = "INSERT INTO Preservation (Description,CreatedAt,UpdatedAt)"
                        + " Values (@Description,@CreatedAt,@UpdatedAt);"
                        + " SELECT * From Preservation Where Id = CAST(SCOPE_IDENTITY() as int)";

            using (var connection = Connection)
            {
                var result = connection.QuerySingleOrDefault<PreservationModel>(sql,
                    new
                    {
                        Description = entity.Description.Value,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                return result;
            }
            
        }

    }
}
