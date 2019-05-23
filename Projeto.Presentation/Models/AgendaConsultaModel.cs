using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Models
{
    public class AgendaConsultaModel
    {        
        public int IdAgenda { get; set; }
        public string NomeTarefa { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int QuantidadeDias { get; set; }
        public string Descricao { get; set; }
    }
}
