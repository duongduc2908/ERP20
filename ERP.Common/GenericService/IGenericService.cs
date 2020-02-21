using ERP.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ERP.Common.GenericService
{
    public interface IGenericService<TEntity>
            where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        TEntity Find(object id);
        TEntity GetLast();

        IEnumerable<TEntity> GetAllIncluing(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        bool Exist(Expression<Func<TEntity, bool>> spec = null);

        int Count(Expression<Func<TEntity, bool>> spec = null);

        void Create(TEntity entity);

        void Delete(TEntity entity);

        void Update(TEntity entity, object id);

        

        void Dispose();
    }
}