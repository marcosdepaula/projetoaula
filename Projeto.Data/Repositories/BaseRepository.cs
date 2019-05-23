using Microsoft.EntityFrameworkCore;
using Projeto.Data.Context;
using Projeto.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projeto.Data.Repositories
{
    public abstract class BaseRepository<TEntity, TKey>
        : IBaseRepository<TEntity, TKey>
        where TEntity : class
    {
        //atributo
        private readonly DataContext context;

        //construtor para injeção de dependência
        public BaseRepository(DataContext context)
        {
            this.context = context;
        }

        public virtual void Add(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Added;
        }

        public virtual void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
        }

        public virtual List<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        public virtual List<TEntity> GetAll(Func<TEntity, bool> where)
        {
            return context.Set<TEntity>().Where(where).ToList();
        }

        public virtual TEntity Get(Func<TEntity, bool> where)
        {
            return context.Set<TEntity>().FirstOrDefault(where);
        }

        public virtual TEntity GetById(TKey id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public virtual void Dispose()
        {
            context.Dispose();
        }
    }
}
