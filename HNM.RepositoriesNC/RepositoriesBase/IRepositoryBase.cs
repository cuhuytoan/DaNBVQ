using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.RepositoriesBase
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Add New Entity
        /// </summary>
        /// <param name="entity"></param>
        void Create(T entity);
        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);
        ValueTask<T> FindAsync(object id);
        T GetById(object id);
        T FirstOrDefault(Expression<Func<T, bool>> expression);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Format URL
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        string FormatURL(string Title);
        string CutText(string TextInput, int NumberCharacter);
        string StripHTML(string input);


    }
}
