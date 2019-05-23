using AutoMapper;
using Projeto.Entities;
using Projeto.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Presentation.Mappings
{
    public class EntityToModelProfile : Profile
    {
        //construtor -> ctor + 2x[tab]
        public EntityToModelProfile()
        {
            CreateMap<Agenda, AgendaConsultaModel>()
                .AfterMap((src, dest) =>
                    dest.QuantidadeDias = Convert.ToInt32(
                        (src.DataFim - src.DataInicio).TotalDays + 1));
        }
    }
}
