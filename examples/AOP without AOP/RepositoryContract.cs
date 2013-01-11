using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace MyCompany.BusinessDomain.CodeContract
{
    [ContractClassFor(typeof(IRepository<>))]
    internal abstract class RepositoryContract<T> : IRepository<T>
        where T : class, IEntity, new()
    {
        #region IRepository<T> Members

        public T GetById<TIdentity>(TIdentity id)
        {
            Contract.Requires<ArgumentNullException>(id != null);

            return default(T);
        }

        public IQueryable<T> AsQueryable()
        {
            Contract.Ensures(Contract.Result<IQueryable<T>>() != null);

            return default(IQueryable<T>);
        }

        public T Insert(T entity)
        {
            Contract.Requires<ArgumentException>(entity != null && entity.IsNew());
            Contract.Ensures(Contract.Result<T>() != null && !entity.IsNew());

            return default(T);
        }

        public T Update(T entity)
        {
            Contract.Requires<ArgumentException>(entity != null && !entity.IsNew());
            Contract.Ensures(Contract.Result<T>() != null && !entity.IsNew());

            return default(T);
        }

        public void Delete(T entity)
        {
            Contract.Requires<ArgumentException>(entity != null && !entity.IsNew());
        }

        public void DeleteById<TIdentity>(TIdentity id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
        }

        public void DeleteByIds<TIdentity>(TIdentity[] ids)
        {
            Contract.Requires<ArgumentNullException>(ids != null);
            Contract.Requires<ArgumentException>(ids.All(id => id != null));
        }

        #endregion
    }
}
