using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Projeto.Presentation.Models; //importando
using Projeto.Presentation.Validations; //importando
using Projeto.Data.Contracts;
using Projeto.Entities;
using AutoMapper;

namespace Projeto.Presentation.Controllers
{
    [Authorize("Bearer")]
    [Produces("application/json")]
    [Route("api/agenda")]
    public class AgendaController : Controller
    {
        //atributo
        private readonly IUnitOfWork unitOfWork;

        //construtor para injeção de dependência
        public AgendaController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] AgendaCadastroModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var agenda = Mapper.Map<Agenda>(model);

                    agenda.Usuario = unitOfWork.UsuarioRepository
                            .Get(u => u.Email.Equals(User.Identity.Name));

                    //gravando o registro da agenda
                    unitOfWork.AgendaRepository.Add(agenda);
                    unitOfWork.SaveChanges();

                    return Ok("Agenda cadastrada com sucesso.");
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }
            else
            {
                return BadRequest(ValidationUtil.GetErrors(ModelState));
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Put([FromBody] AgendaEdicaoModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //obtendo o usuario autenticado
                    var usuario = unitOfWork.UsuarioRepository
                        .Get(u => u.Email.Equals(User.Identity.Name));

                    //verificar se o registro enviado pertence a este usuário
                    if (unitOfWork.AgendaRepository
                        .Get(a => a.IdAgenda == model.IdAgenda
                               && a.Usuario.IdUsuario == usuario.IdUsuario) != null)
                    {
                        var agenda = Mapper.Map<Agenda>(model);
                        agenda.Usuario = usuario;

                        unitOfWork.AgendaRepository.Update(agenda);
                        unitOfWork.SaveChanges();

                        return Ok("Agenda atualizada com sucesso.");
                    }
                    else
                    {
                        return BadRequest("Registro de Agenda é inválido.");
                    }
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }
            else
            {
                return BadRequest(ValidationUtil.GetErrors(ModelState));
            }
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int idAgenda)
        {
            try
            {
                //obtendo o usuario autenticado
                var usuario = unitOfWork.UsuarioRepository
                    .Get(u => u.Email.Equals(User.Identity.Name));

                //verificar se o registro enviado pertence a este usuário
                if (unitOfWork.AgendaRepository
                    .Get(a => a.IdAgenda == idAgenda
                           && a.Usuario.IdUsuario == usuario.IdUsuario) != null)
                {
                    var agenda = unitOfWork.AgendaRepository.GetById(idAgenda);

                    unitOfWork.AgendaRepository.Delete(agenda);
                    unitOfWork.SaveChanges();

                    return Ok("Agenda excluída com sucesso.");
                }
                else
                {
                    return BadRequest("Registro de Agenda é inválido.");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            try
            {
                //obtendo o usuario autenticado
                var usuario = unitOfWork.UsuarioRepository
                    .Get(u => u.Email.Equals(User.Identity.Name));

                //obter uma lista de todos os registros na agenda relacionados
                //ao usuário que está autenticado..
                var lista = unitOfWork.AgendaRepository
                    .GetAll(a => a.Usuario.IdUsuario == usuario.IdUsuario);

                return Ok(Mapper.Map<List<AgendaConsultaModel>>(lista));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{idAgenda}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetById(int idAgenda)
        {
            try
            {
                //obtendo o usuario autenticado
                var usuario = unitOfWork.UsuarioRepository
                    .Get(u => u.Email.Equals(User.Identity.Name));

                //verificar se o registro da agenda pertence a este usuario
                if (unitOfWork.AgendaRepository
                    .Get(a => a.IdAgenda == idAgenda
                           && a.Usuario.IdUsuario == usuario.IdUsuario) != null)
                {
                    return Ok(Mapper.Map<AgendaConsultaModel>
                        (unitOfWork.AgendaRepository.GetById(idAgenda)));
                }
                else
                {
                    return BadRequest("Registro de Agenda é inválido.");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}