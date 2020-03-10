using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace DataAccessLayer
{
    public interface IRepository<T>
    {
        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        //IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(
        //    IQueryable<TEntity> source,
        //    Expression<Func<TEntity, TProperty>> navigationPropertyPath);

        IQueryable<T> Find(Expression<Func<T, bool>> expression);

        IQueryable<T> Find<TProperty, TProperty2>(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, TProperty>> property1,
            Expression<Func<T, TProperty2>> property2);
    }
}