using System;
using System.IO;
using AutoMapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Protecta.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Protecta.Infrastructure.Connection;
using Microsoft.Extensions.Configuration;
using Protecta.Application.Service.Services;
using Protecta.Application.Service.Helpers;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Protecta.Application.Service.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Protecta.CrossCuting.Utilities.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Protecta.Application.Service.Services.UserModule;
using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;
using Protecta.Infrastructure.Data.UserModule.Repositories;
using Protecta.Application.Service.Services.PlanillaModule;
using Protecta.Domain.Service.PlanillaModule.Aggregates.PlanillaAgg;
using Protecta.Application.Service.Services.StateModule;
using Protecta.Domain.Service.StateModule.Aggregates.StateAgg;
using Protecta.Infrastructure.Data.StateModule.Repositories;
using Protecta.Application.Service.Services.ProductoModule;
using Protecta.Domain.Service.ProductoModule.Aggregates.ProductoAgg;
using Protecta.Infrastructure.Data.ProductoModule.Repositories;
using Protecta.Application.Service.Services.EntidadModule;
using Protecta.Infrastructure.Data.EntidadModule.Repositories;
using Protecta.Domain.Service.EntidadModule.Aggregates.EntidadAgg;
using Protecta.Application.Service.Services.ConciliacionModule;
using Protecta.Domain.Service.ConciliacionModule.Aggregates.ConciliacionAgg;
using Protecta.Infrastructure.Data.ConciliacionModule.Repositories;
using Protecta.Domain.Service.MonedaModule.Aggregates.MonedaAgg;
using Protecta.Infrastructure.Data.MonedaModule.Repositories;
using Protecta.Application.Service.Services.MonedaService;
using Protecta.Application.Service.Services.DepositoModule;
using Protecta.Domain.Service.DepositoModule.Aggregates.DepositoAgg;
using Protecta.Infrastructure.Data.DepositoModule.Repositories;
using Protecta.Application.Service.Services.PerfilModule;
using Protecta.Domain.Service.PerfilModule.Aggregates.PerfilAgg;
using Protecta.Infrastructure.Data.PerfilModule.Repositories;
using Protecta.Application.Service.Services.TipoArchivoModule;
using Protecta.Domain.Service.TipoArchivoModule.Aggregates.TipoArchivoAgg;
using Protecta.Infrastructure.Data.TipoArchivoModule.Repositories;
using Protecta.Application.Service.Services.GeneracionExactusModule;
using Protecta.Domain.Service.GeneracionExactusModule.Aggregates.GeneracionExactusAgg;
using Protecta.Infrastructure.Data.GeneracionExactusModule.Repositories;
using Protecta.Application.Service.Services.ReporteService;
using Protecta.Domain.Service.ReporteModule.Aggregates.ReporteAgg;
using Protecta.Infrastructure.Data.ReporteModule.Repositories;
using Protecta.Application.Service.Services.CanalModule;
using Protecta.Domain.Service.CanalModule.Aggregates.CanalAgg;
using Protecta.Infrastructure.Data.CanalModule.Repositories;
using Protecta.Application.Service.Services.FacturaModule;
using Protecta.Domain.Service.FacturaModule.Aggregates.FacturaAgg;
using Protecta.Infrastructure.Data.FacturaModule.Repositories;
using Protecta.Application.Service.Services.GeneracionArchivosModule;
using Protecta.Infrastructure.Data.GeneracionArchivosModule.Repositories;
using Protecta.Domain.Service.GeneracionArchivosModule.Aggregates.GeneracionArchivosAgg;
using Protecta.Infrastructure.Data.CobranzasModule.Repositories;
using Protecta.Domain.Service.CobranzaModule.Aggregates.CobranzaAgg;
using Protecta.Application.Service.Services.CobranzasModule;
using Protecta.Application.Service.Services.CuponeraModule;
using Protecta.Domain.Service.CuponeraModule.Aggregates.CuponeraAgg;
using Protecta.Infrastructure.Data.CuponeraModule.Repositories;
using Microsoft.Extensions.FileProviders;

namespace slAngularWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            loggerFactory.ConfigureNLog(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));


            services.ConfigureLoggerService();
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.AddMvc();
            services.AddAutoMapper();

            // Configurar objetos de configuración fuertemente tipados
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // Configurar objetos Ldap de appsettings
            var appSettingsLdap = Configuration.GetSection("Ldap");
            services.Configure<Ldap>(appSettingsLdap);

            // Configurar jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //Configura Inyeccion de dependencias (DI) para servicio de aplicaciones
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IPlanillaService, PlanillaService>();
            services.AddScoped<IEntidadService, EntidadService>();
            services.AddScoped<IConciliacionService, ConciliacionService>();
            services.AddScoped<IMonedaService, MonedaService>();
            services.AddScoped<IDepositoService, DepositoService>();
            services.AddScoped<IPerfilService, PerfilService>();
            services.AddScoped<ITipoArchivoService, TipoArchivoService>();
            services.AddScoped<IGeneracionExactusService, GeneracionExactusService>();
            services.AddScoped<IGeneracionArchivosService, GeneracionArchivosService>();
            services.AddScoped<IReporteService, ReporteService>();
            services.AddScoped<ICanalService, CanalService>();
            services.AddScoped<IFacturaService, FacturaService>();
            services.AddScoped<ICobranzasService, CobranzasService>();
            services.AddScoped<ICuponeraService, CuponeraService>();

            //Configura Inyeccion de dependencias (DI) para los repositorios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<IPlanillaRepository, PlanillaRepository>();
            services.AddScoped<IEntidadRepository, EntidadRepository>();
            services.AddScoped<IConciliacionRepository, ConciliacionRepository>();
            services.AddScoped<IMonedaRepository, MonedaRepository>();
            services.AddScoped<IDepositoRepository, DepositoRepository>();
            services.AddScoped<IPerfilRepository, PerfilRepository>();
            services.AddScoped<ITipoArchivoRepository, TipoArchivoRepository>();
            services.AddScoped<IGeneracionExactusRepository, GeneracionExactusRepository>();
            services.AddScoped<IGeneracionArchivosRepository, GeneracionArchivosRepository>();
            services.AddScoped<IReporteRepository, ReporteRepository>();
            services.AddScoped<ICanalRepository, CanalRepository>();
            services.AddScoped<IFacturaRepository, FacturaRepository>();
            services.AddScoped<ICobranzaRepository, CobranzaRepository>();
            services.AddScoped<ICuponeraRepository, CuponeraRepository>();
            services.AddScoped<IConnectionBase, ConnectionBase>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
