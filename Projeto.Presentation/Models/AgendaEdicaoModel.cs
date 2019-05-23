using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Projeto.Presentation.Models
{
    public class AgendaEdicaoModel
    {
        [Required(ErrorMessage = "Campo obrigatório.")]
        public int IdAgenda { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string NomeTarefa { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public DateTime DataFim { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Descricao { get; set; }
    }
}
