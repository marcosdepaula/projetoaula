using Projeto.Data.Context;
using Projeto.Data.Contracts;
using Projeto.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Repositories
{
    public class AgendaRepository
        : BaseRepository<Agenda, int>, IAgendaRepository
    {
        //atributo
        private readonly DataContext context;

        public AgendaRepository(DataContext context)
            : base(context)
        {
            this.context = context;
        }
    }
}
