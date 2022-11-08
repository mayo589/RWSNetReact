using System;
using System.Linq;
using System.Linq.Expressions;

namespace TranslationManagement.Api.Repositories
{
    /// <summary>
    /// Interface for main repository class used in this application
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryBase<T>
    {
        T FindById(int id);

        IQueryable<T> FindAll();

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
