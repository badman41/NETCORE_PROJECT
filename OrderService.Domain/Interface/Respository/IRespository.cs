using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrderService.Domain.Interface.Respository
{
    public interface IRepository<TReadEntity,TWriteEntity> where TReadEntity : class where TWriteEntity : class
    {
        TReadEntity Get(int id);
        IEnumerable<TReadEntity> All();
        IEnumerable<TReadEntity> Find(Expression<Func<TReadEntity, bool>> predicate);
        //void Add(TWriteEntity entity);
        //void Add(TWriteEntity entity,int userId);
        bool Update(TWriteEntity entity);
        bool Delete(int id);
        TReadEntity Add(TWriteEntity entity);
    }
}
