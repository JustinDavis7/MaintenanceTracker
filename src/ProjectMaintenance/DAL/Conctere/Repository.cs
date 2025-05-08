using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ProjectMaintenance.DAL.Abstract;

namespace ProjectMaintenance.DAL.Concrete
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext ctx)
        {
            _context = ctx;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual TEntity FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual bool Exists(int id)
        {
            return FindById(id) != null;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> dbs = _dbSet;
            foreach (var item in includes)
            {
                dbs = dbs.Include(item);
            }
            return dbs;
        }

        public virtual TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new Exception("Entity to delete was not found or was null");
            }
            else
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }

        public virtual void DeleteById(int id)
        {
            Delete(FindById(id));
        }
    }
}
