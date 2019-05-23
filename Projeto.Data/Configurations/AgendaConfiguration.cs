using Microsoft.EntityFrameworkCore;
using Projeto.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Projeto.Data.Configurations
{
    public class AgendaConfiguration
        : IEntityTypeConfiguration<Agenda>
    {
        public void Configure(EntityTypeBuilder<Agenda> builder)
        {
            //chave primária
            builder.HasKey(a => new { a.IdAgenda });

            builder.Property(a => a.NomeTarefa)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(a => a.DataInicio)
                .IsRequired();

            builder.Property(a => a.DataFim)
                .IsRequired();

            builder.Property(a => a.Descricao)
                .HasMaxLength(1000)
                .IsRequired();

            //mapeamento do relacionamento
            //'1' usuário para 'N' Agenda
            builder.HasOne(a => a.Usuario)
                .WithMany(u => u.Agendas);
        }
    }
}
