using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data.Context
{
    public class DataContextFactory
        : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(@"C:\Curso C# (A)\Aula03\Projeto.Presentation\appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseSqlServer(config.GetConnectionString("aula"));

            return new DataContext(builder.Options);
        }
    }
}
