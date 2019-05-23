using System;
using System.Collections.Generic;

namespace Projeto.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public virtual ICollection<Agenda> Agendas { get; set; }
    }
}
