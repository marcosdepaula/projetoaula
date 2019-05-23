using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //validações

namespace Projeto.Presentation.Models
{
    public class UsuarioCadastroModel
    {
        [MinLength(6, ErrorMessage = "Mínimo de {1} caracteres.")]
        [MaxLength(100, ErrorMessage = "Máximo de {1} caracteres.")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Nome { get; set; }

        [EmailAddress(ErrorMessage = "Endereço de email inválido.")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Email { get; set; }

        [RegularExpression("^[A-Za-z0-9@&]{6,20}$", ErrorMessage = "Senha inválida")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "Senhas não conferem.")]
        [Required(ErrorMessage = "Campo obrigatório.")]
        public string SenhaConfirm { get; set; }
    }
}
