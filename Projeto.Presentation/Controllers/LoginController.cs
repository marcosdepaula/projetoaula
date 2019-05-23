using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projeto.Presentation.Models; //importando
using Projeto.Presentation.Validations; //importando
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Principal;
using Projeto.Data.Contracts;
using Projeto.Data.Utils;

namespace Projeto.Presentation.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : Controller
    {
        //atributo
        private readonly IUnitOfWork unitOfWork;

        //construtor para injeção de dependência
        public LoginController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //serviço para realizar a autenticação do usuário
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public object Post([FromBody] LoginModel model,
                           [FromServices] TokenConfiguration tokenConfiguration,
                           [FromServices] LoginConfiguration loginConfiguration)
        {
            if(ModelState.IsValid)
            {
                //criptografando a senha recebida
                model.Senha = Criptografia.GetMD5Hash(model.Senha);

                //buscar o usuario pelo login e senha
                var usuario = unitOfWork.UsuarioRepository
                                .Get(u => u.Email.Equals(model.Email)
                                       && u.Senha.Equals(model.Senha));

                if (usuario != null) //se o usuário foi encontrado
                {
                    //criando as credenciais do usuario..
                    ClaimsIdentity identity = new ClaimsIdentity(
                            new GenericIdentity(usuario.Email, "Login"),
                            new[]
                            {
                                //registrando que o email representa o USERNAME do usuario..
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                                new Claim(JwtRegisteredClaimNames.UniqueName, model.Email)
                            }
                        );

                    //gerando o token
                    var dataCriacao = DateTime.Now;
                    var dataExpiracao = dataCriacao + TimeSpan.FromSeconds(tokenConfiguration.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new
                    SecurityTokenDescriptor
                    {
                        Issuer = tokenConfiguration.Issuer,
                        Audience = tokenConfiguration.Audience,
                        SigningCredentials = loginConfiguration.SigningCredentials,
                        Subject = identity,
                        NotBefore = dataCriacao,
                        Expires = dataExpiracao
                    });

                    var token = handler.WriteToken(securityToken); //CRIADO!!

                    return new
                    {
                        authenticated = true,
                        created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                        expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                        accessToken = token,
                        message = "OK"
                    };
                }
                else
                {
                    return BadRequest(
                            new
                            {
                                authenticated = false,
                                message = "Acesso negado. Usuário inválido."
                            }
                        );
                }
            }
            else
            {
                return BadRequest(ValidationUtil.GetErrors(ModelState));
            }
        }
    }
}