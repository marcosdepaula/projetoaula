using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Projeto.Presentation.Filters;
using Projeto.Data.Contracts;
using Projeto.Data.Repositories;
using Projeto.Data.Context;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Projeto.Presentation.Mappings;

namespace Projeto.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //mapeamento das dependencias do repositório
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IAgendaRepository, AgendaRepository>();
            services.AddTransient<DataContext>();

            //configurar para o DataContext o caminho de uma connectionstring
            //a connectionString estará mapeada no arquivo 'appsettings.json'
            services.AddDbContext<DataContext>(
                    options => options.UseSqlServer(Configuration.GetConnectionString("aula"))
                );


            //mapear as classes 'Profile' do AutoMapper
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<EntityToModelProfile>();
                cfg.AddProfile<ModelToEntityProfile>();
            });


            services.AddMvc();

            var signingConfigurations = new LoginConfiguration();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                Configuration.GetSection("TokenConfiguration"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            //configurando o gerador de documentação
            //da api através do framework SWAGGER
            services.AddSwaggerGen(
                    s =>
                    {
                        s.SwaggerDoc("v1",
                            new Info
                            {
                                Title = "Projeto Agenda de Usuários",
                                Description = "Curso C# WebDeveloper Avançado",
                                Version = "v1",
                                Contact = new Contact
                                {
                                    Name = "COTI Informática",
                                    Email = "contato@cotiinformatica.com.br",
                                    Url = "http://www.cotiinformatica.com.br"
                                }
                            });

                        s.OperationFilter<AddHeaderParameter>();
                    }
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            //registrando o swagger
            app.UseSwagger();
            app.UseSwaggerUI(
                s => 
                {
                    s.SwaggerEndpoint("/swagger/v1/swagger.json", "projeto");
                });
        }
    }
}
