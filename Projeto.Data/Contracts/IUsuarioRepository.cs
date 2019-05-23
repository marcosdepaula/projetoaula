using System;
using System.Collections.Generic;
using System.Text;
using Projeto.Entities;

namespace Projeto.Data.Contracts
{
    public interface IUsuarioRepository
        : IBaseRepository<Usuario, int>
    {

    }
}
