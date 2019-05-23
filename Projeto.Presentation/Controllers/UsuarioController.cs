using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projeto.Presentation.Models; //importando
using Projeto.Presentation.Validations; //importando
using Projeto.Data.Contracts;
using AutoMapper;
using Projeto.Entities;

namespace Projeto.Presentation.Controllers
{
    [Produces("application/json")]
    [Route("api/usuario")]
    public class UsuarioController : Controller
    {
        //atributo
        private readonly IUnitOfWork unitOfWork;

        //construtor para injeção de dependência
        public UsuarioController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //criando um serviço para cadastro de usuários
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] UsuarioCadastroModel model)
        {
            //verificar se o conteúdo do objeto está correto
            //em relação às regras de validação
            if(ModelState.IsValid)
            {
                try
                {
                    //verificar se o email informado não existe na base de dados
                    if(unitOfWork.UsuarioRepository
                        .Get(u => u.Email.Equals(model.Email)) == null)
                    {
                        var usuario = Mapper.Map<Usuario>(model);

                        unitOfWork.UsuarioRepository.Add(usuario);
                        unitOfWork.SaveChanges();

                        return Ok("Usuário cadastrado com sucesso.");
                    }
                    else
                    {
                        return BadRequest($"O email '{model.Email}' já existe no sistema.");
                    }
                }
                catch(Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }
            else
            {
                return BadRequest(ValidationUtil.GetErrors(ModelState));
            }
        }

    }
}