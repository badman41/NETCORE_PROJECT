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
    public class AddressRepository : BaseRepository, IAddressRepository
    {
        public AddressRepository(IConfiguration config) : base(config)
        {
        }

        public IEnumerable<AddressModel> Find(Expression<Func<AddressModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public AddressModel Get(int id)
        {
            string sql = "SELECT * FROM Address Where Id =@Id;";

            using (var connection = Connection)
            {
                var affectedRows = connection.QuerySingle<AddressModel>(sql,
                    new
                    {
                        Id = id
                    });
                return affectedRows;
            }
        }
        public IEnumerable<AddressModel> All()
        {
            using (var cn = Connection)
            {
                var Addresss = cn.Query<AddressModel>("SELECT * FROM Address");
                return Addresss;
            }
        }

        public bool Delete(int id)
        {
            string sql = "DELETE Address Where Id =@Id;";

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

        public bool Update(Address entity)
        {
            throw new NotImplementedException();
        }

        public void Add(Address entity, int userId)
        {
        }

        public AddressModel Add(Address entity)
        {
            string sql = "INSERT INTO Address (City,Country,District,Lat,Lng,Street,StreetNumber)"
                        + " values (@City,@Country,@District,@Lat,@Lng,@Street,@StreetNumber);" 
                        + " SELECT * From Address Where Id = CAST(SCOPE_IDENTITY() as int)";

            using (var connection = Connection)
            {
                var result = connection.QuerySingleOrDefault<AddressModel>(sql,
                    new
                    {
                        City = entity.City.Value,
                        Country = entity.Country.Value,
                        District = entity.District.Value,
                        Lat = entity.Lat.Value,
                        Lng = entity.Lng.Value,
                        Street = entity.Street.Value,
                        StreetNumber = entity.StreetNumber.Value
                    });
                return result;
            }
            
        }

        public AddressModel GetByLatLng(string lat, string lng)
        {
            string sql = "SELECT * FROM Address Where Lat =@Lat and Lng = @Lng;";

            using (var connection = Connection)
            {
                var affectedRows = connection.QueryFirstOrDefault<AddressModel>(sql,
                    new
                    {
                        Lat = lat,
                        Lng = lng
                    });
                return affectedRows;
            }
        }
    }
}
