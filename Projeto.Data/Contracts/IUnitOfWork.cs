using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();

        void SaveChanges();

        IAgendaRepository AgendaRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
    }
}
