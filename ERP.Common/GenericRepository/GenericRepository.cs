using ERP.Common.Models;
using ERP.Data.DbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Reflection;

namespace ERP.Common.GenericRepository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private const string ParamNull = "Entity input can't null";

        protected readonly ERPDbContext _dbContext;

        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ERPDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public ICollection<TEntity> GetAll()
        {
            var a =  _dbSet.ToList();
            return a;
        }


        public TEntity GetByID(object Id)
        {
            return _dbSet.Find(Id);
        }
        public TEntity GetLast()
        {
            int t = _dbSet.Count();
            if (t == 0)
            {
                return null;
            }
            var m = _dbSet.ToList()[t-1];
            return m ;
        }


        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Update(TEntity entity, object id)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            TEntity exist = _dbSet.Find(id);
            if (exist != null)
            {
                _dbContext.Entry(exist).CurrentValues.SetValues(entity);
                _dbContext.SaveChanges();
            }
        }

        public int Count(Expression<Func<TEntity, bool>> spec = null)
        {
            return (spec == null ? _dbSet.Count() : _dbSet.Count(spec));
        }

        public bool Exist(Expression<Func<TEntity, bool>> spec = null)
        {
            return (spec == null ? _dbSet.Any() : _dbSet.Any(spec));
        }

        public void SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}