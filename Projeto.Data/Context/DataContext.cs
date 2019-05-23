using Microsoft.EntityFrameworkCore;
using Projeto.Data.Configurations;
using Projeto.Data.Utils;
using Projeto.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Context
{
    public class DataContext : DbContext
    {
        //construtor com injeção de dependência
        //que será definida na classe 'Startup.cs'
        public DataContext(DbContextOptions<DataContext> builder)
            : base(builder)
        {

        }

        //sobrescrita do método OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //adicionar cada classe de configuração
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new AgendaConfiguration());

            //incluindo um registro de usuario na base de dados
            modelBuilder.Entity<Usuario>().HasData(
                    new Usuario
                    {
                        IdUsuario = 1,
                        Nome = "Sergio Mendes",
                        Email = "sergio.coti@gmail.cokm",
                        Senha = Criptografia.GetMD5Hash("adminadmin")
                    }
                );
        }

        //propriedades DbSet<TEntity>
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Agenda> Agenda { get; set; }
    }
}
