using ERP.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ERP.Common.GenericRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        ICollection<TEntity> GetAll();
        TEntity GetLast();
        int Count(Expression<Func<TEntity, bool>> spec = null);

        bool Exist(Expression<Func<TEntity, bool>> spec = null);

        TEntity GetByID(object Id);

        void Create(TEntity entity);

        void Delete(object id);

        void Delete(TEntity entity);

        void Update(TEntity entity, object id);

        void SaveChanges();

        void Dispose();
    }
}