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
            // ReSharper disable InvocationIsSkipped
            // ReSharper disable CompareNonConstrainedGenericWithNull
            Contract.Requires<ArgumentNullException>(id != null);
            // ReSharper restore CompareNonConstrainedGenericWithNull
            // ReSharper restore InvocationIsSkipped

            return default(T);
        }

        public IQueryable<T> AsQueryable()
        {
            // ReSharper disable InvocationIsSkipped
            Contract.Ensures(Contract.Result<IQueryable<T>>() != null);
            // ReSharper restore InvocationIsSkipped

            return default(IQueryable<T>);
        }

        public T Insert(T entity)
        {
            // ReSharper disable InvocationIsSkipped
            Contract.Requires<ArgumentException>(entity != null && entity.IsNew());
            Contract.Ensures(Contract.Result<T>() != null && !entity.IsNew());
            // ReSharper restore InvocationIsSkipped

            return default(T);
        }

        public T Update(T entity)
        {
            // ReSharper disable InvocationIsSkipped
            Contract.Requires<ArgumentException>(entity != null && !entity.IsNew());
            Contract.Ensures(Contract.Result<T>() != null && !entity.IsNew());
            // ReSharper restore InvocationIsSkipped

            return default(T);
        }

        public void Delete(T entity)
        {
            // ReSharper disable InvocationIsSkipped
            Contract.Requires<ArgumentException>(entity != null && !entity.IsNew());
            // ReSharper restore InvocationIsSkipped
        }

        public void DeleteById<TIdentity>(TIdentity id)
        {
            // ReSharper disable InvocationIsSkipped
            // ReSharper disable CompareNonConstrainedGenericWithNull
            Contract.Requires<ArgumentNullException>(id != null);
            // ReSharper restore CompareNonConstrainedGenericWithNull
            // ReSharper restore InvocationIsSkipped
        }

        public void DeleteByIds<TIdentity>(TIdentity[] ids)
        {
            // ReSharper disable InvocationIsSkipped
            // ReSharper disable CompareNonConstrainedGenericWithNull
            Contract.Requires<ArgumentNullException>(ids != null);
            Contract.Requires<ArgumentException>(ids.All(id => id != null));
            // ReSharper restore CompareNonConstrainedGenericWithNull
            // ReSharper restore InvocationIsSkipped
        }

        #endregion
    }
}
