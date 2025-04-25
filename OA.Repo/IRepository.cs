using OA.Data;
using System.Collections.Generic;

namespace OA.Repo
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets all the rows and valaues within the specified object table
        /// </summary>
        /// <example></example>
        /// <returns></returns>
        IEnumerable<T> GetAll();
        T Get(long id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Remove(T entity);
        void SaveChanges();
    }
}