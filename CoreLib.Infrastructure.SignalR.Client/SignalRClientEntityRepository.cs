using CoreLib.Patterns.Repository.Abstraction;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLib.Infrastructure.SignalR.Client
{
    public class SignalRClientEntityRepository<T> : baseSignalRClientService, IEntityRepository<T>
        where T : IEntity
    {
        #region Constructors
        public SignalRClientEntityRepository(string url) : base(url)
        {
        } 
        #endregion

        #region Public Functions
        public void Create(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if ((_connection == null)) throw new NullReferenceException(nameof(_connection));
            #endregion

            CreateAsync(entity).GetAwaiter().GetResult();
        }
        public async Task CreateAsync(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if ((_connection == null)) throw new NullReferenceException(nameof(_connection));
            #endregion

            string methodName = nameof(CreateAsync);
            await _connection.SendAsync(methodName, entity);
        }

        public void Update(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if ((_connection == null)) throw new NullReferenceException(nameof(_connection));
            #endregion

            UpdateAsync(entity).GetAwaiter().GetResult();
        }
        public async Task UpdateAsync(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if ((_connection == null)) throw new NullReferenceException(nameof(_connection));
            #endregion

            string methodName = nameof(UpdateAsync);
            await _connection.SendAsync(methodName, entity);
        }

        public T FindById(int id)
        {
            #region Guards
            if ((_connection == null)) throw new NullReferenceException(nameof(_connection));
            #endregion

            return FindByIdAsync(id).GetAwaiter().GetResult();
        }
        public async Task<T> FindByIdAsync(int id)
        {
            #region Guards
            if ((_connection == null)) throw new NullReferenceException(nameof(_connection));
            #endregion

            string methodName = nameof(FindByIdAsync);
            return await _connection.InvokeAsync<T>(methodName, id);
        }

        public void Delete(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if ((_connection == null)) throw new NullReferenceException(nameof(_connection));
            #endregion

            DeleteAsync(entity).GetAwaiter().GetResult();
        }
        public async Task DeleteAsync(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if ((_connection == null)) throw new NullReferenceException(nameof(_connection));
            #endregion

            string methodName = nameof(DeleteAsync);
            await _connection.SendAsync(methodName, entity);
        }

        public IEnumerator<T> GetEnumerator()
        {
            #region Guards
            if ((_connection == null)) throw new NullReferenceException(nameof(_connection));
            #endregion

            IEnumerable<T> result = read().GetAwaiter().GetResult();
            return result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Private Functions
        protected async Task<IEnumerable<T>> read()
        {
            string methodName = nameof(read);
            return await _connection.InvokeAsync<IEnumerable<T>>(methodName);
        }
        #endregion
    }
}
