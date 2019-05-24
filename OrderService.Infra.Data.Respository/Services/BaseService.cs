using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OrderService.Infra.Data.Respository.Services
{
    public class BaseService<TReadEntity,TWriteEntity> : IService<TReadEntity, TWriteEntity> where TReadEntity : class where TWriteEntity : class
    {
        private readonly IRepository<TReadEntity, TWriteEntity> _repository;

        public BaseService(IRepository<TReadEntity, TWriteEntity> repository)
        {
            _repository = repository;
        }
        protected IRepository<TReadEntity, TWriteEntity> Repository
        {
            get { return _repository; }
        }
        public virtual TReadEntity Get(int id)
        {
            return _repository.Get(id);
        }

        public virtual IEnumerable<TReadEntity> All()
        {
            return _repository.All();
        }

        public virtual IEnumerable<TReadEntity> Find(Expression<Func<TReadEntity, bool>> predicate)
        {
            return _repository.Find(predicate);
        }

        #region CRUD
        public virtual bool Add(TWriteEntity entity)
        {
            return false;
        }

        public virtual bool Update(TWriteEntity entity)
        {
            return false;
        }

        public virtual bool Delete(TWriteEntity entity)
        {
            return false;
        }

        public bool Add(TWriteEntity entity, int userId)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}

