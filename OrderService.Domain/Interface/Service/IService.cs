using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OrderService.Domain.Interface.Service
{
    public interface IService<TReadEntity,TWriteEntity> where TReadEntity : class where TWriteEntity : class
    {
        TReadEntity Get(int id);
        IEnumerable<TReadEntity> All();
        IEnumerable<TReadEntity> Find(Expression<Func<TReadEntity, bool>> predicate);
        bool Add(TWriteEntity entity);
        bool Add(TWriteEntity entity, int userId);
        bool Update(TWriteEntity entity);
        bool Delete(TWriteEntity entity);
    }
}
