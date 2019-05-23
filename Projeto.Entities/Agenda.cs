using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Entities
{
    public class Agenda
    {
        public int IdAgenda { get; set; }
        public string NomeTarefa { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Descricao { get; set; }

        //Navegabilidade
        public virtual Usuario Usuario { get; set; }
    }
}
