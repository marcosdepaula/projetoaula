using Microsoft.EntityFrameworkCore.Storage;
using Projeto.Data.Context;
using Projeto.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        //atributo
        private readonly DataContext context;
        private IDbContextTransaction transaction;

        //construtor para injeção de dependência
        public UnitOfWork(DataContext context)
        {
            this.context = context;
        }

        public void BeginTransaction()
        {
            transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public IAgendaRepository AgendaRepository
            => new AgendaRepository(context);

        public IUsuarioRepository UsuarioRepository
            => new UsuarioRepository(context);

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
