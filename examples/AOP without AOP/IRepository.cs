using System.Diagnostics.Contracts;
using System.Linq;
using MyCompany.CodeContract;

namespace MyCompany.BusinessDomain
{
    /// <summary>
    /// Repository pattern interface
    /// </summary>
    /// <typeparam name="T">IEntity implementation</typeparam>
    [ContractClass(typeof(RepositoryContract<>))]
    public interface IRepository<T>
        where T : class, IEntity, new()
    {
        /// <summary>
        /// Gets entity by id. Will returns null if there is no such entity.
        /// </summary>
        /// <typeparam name="TIdentity">Identity type</typeparam>
        /// <param name="id">The id</param>
        /// <returns>Requested entity</returns>
        T GetById<TIdentity>(TIdentity id);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns><see cref="IQueryable"/> entities</returns>
        IQueryable<T> AsQueryable();

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity after insert.</returns>
        T Insert(T entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity after update</returns>
        T Update(T entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes the by id.
        /// </summary>
        /// <typeparam name="TIdentity">Identity type</typeparam>
        /// <param name="id">The id.</param>
        void DeleteById<TIdentity>(TIdentity id);

        /// <summary>
        /// Deletes the by ids.
        /// </summary>
        /// <typeparam name="TIdentity">Identity type</typeparam>
        /// <param name="ids">The ids.</param>
        void DeleteByIds<TIdentity>(TIdentity[] ids);
    }
}
