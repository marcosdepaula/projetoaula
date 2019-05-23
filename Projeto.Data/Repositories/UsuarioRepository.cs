using Projeto.Data.Context;
using Projeto.Data.Contracts;
using Projeto.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Repositories
{
    public class UsuarioRepository
        : BaseRepository<Usuario, int>, IUsuarioRepository
    {
        //atributo
        private readonly DataContext context;

        //construtor para injeção de dependencia
        public UsuarioRepository(DataContext context)
            : base(context)
        {
            this.context = context;
        }
    }
}
