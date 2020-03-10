using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DataAccessLayer
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly DbSet<T> _dbSet;

        public Repository(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
            _dbSet = _dataBaseContext.Set<T>();
        }

        public void Create(T entity1)
        {
            _dbSet.Add(entity1);
            _dataBaseContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _dataBaseContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _dataBaseContext.SaveChanges();
        }

        //public IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(
        //    IQueryable<TEntity> source,
        //    Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        //    where TEntity : class
        //{
        //    return source.Include(navigationPropertyPath);
        //}

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public IQueryable<T> Find<TProperty, TProperty2>(
            Expression<Func<T, bool>> expression, 
            Expression<Func<T, TProperty>> property1,
            Expression<Func<T, TProperty2>> property2)
        {
            return _dbSet.Include(property1).Include(property2).Where(expression);
        }
    }
}