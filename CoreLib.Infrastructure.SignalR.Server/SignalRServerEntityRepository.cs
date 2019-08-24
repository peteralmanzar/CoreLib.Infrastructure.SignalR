using CoreLib.Patterns.Repository.Abstraction;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLib.Infrastructure.SignalR.Server
{
    public class SignalRServerEntityRepository<T> : Hub, IEntityRepository<T>
        where T : IEntity
    {
        #region Members
        protected IEntityRepository<T> _repository;
        #endregion

        #region Constructors
        public SignalRServerEntityRepository(IEntityRepository<T> repository)
        {
            #region Guards
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            #endregion

            _repository = repository;
        }
        #endregion

        #region Guards
        public void Create(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            #endregion

            CreateAsync(entity).GetAwaiter().GetResult();
        }

        public async Task CreateAsync(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            #endregion

            _repository.Create(entity);
            await Clients.All.SendAsync(nameof(Create), entity);
        }

        public void Delete(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            #endregion

            DeleteAsync(entity).GetAwaiter().GetResult();
        }

        public async Task DeleteAsync(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            #endregion

            _repository.Delete(entity);
            await Clients.All.SendAsync(nameof(Delete), entity);
        }

        public T FindById(int id)
        {
            return FindByIdAsync(id).GetAwaiter().GetResult();
        }

        private async Task<T> FindByIdAsync(int id)
        {
            return await Task.Run<T>(() => { return _repository.FindById(id); });
        }

        public IEnumerator<T> GetEnumerator()
        {
            #region Guards
            if (_repository == null) throw new ArgumentNullException(nameof(_repository));
            #endregion

            return _repository.GetEnumerator();
        }

        public void Update(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            #endregion

            UpdateAsync(entity).GetAwaiter().GetResult();
        }

        private async Task UpdateAsync(T entity)
        {
            #region Guards
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            #endregion

            _repository.Update(entity);
            await Clients.All.SendAsync(nameof(Update), entity);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        } 
        #endregion
    }
}
