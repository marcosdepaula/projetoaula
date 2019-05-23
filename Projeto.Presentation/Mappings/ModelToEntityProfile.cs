using AutoMapper;
using Projeto.Data.Utils;
using Projeto.Entities;
using Projeto.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Mappings
{
    public class ModelToEntityProfile : Profile
    {
        //construtor -> ctor + 2x[tab]
        public ModelToEntityProfile()
        {
            CreateMap<UsuarioCadastroModel, Usuario>()
                .AfterMap((src, dest) =>
                    dest.Senha = Criptografia.GetMD5Hash(src.Senha));

            CreateMap<AgendaCadastroModel, Agenda>();

            CreateMap<AgendaEdicaoModel, Agenda>();
        }
    }
}
