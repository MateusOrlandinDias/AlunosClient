using AlunosApi.Context;
using AlunosApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlunosApi
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
            /*
             * � importante criar o servi�o de contexto de conex�o com o banco de dados utilizando o comando
             * options.UseSqlServer("STRINGCONNECTION")
             * Nesse caso a string connection vem do arquivo appsettings.json
             */
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            /*
             * � importante registrar o service concluido neste startup.cs
             * 
             * Nesta proxima linha de c�digo eu estou pedindo que sempre que eu use o AlunoService, ele
             * vai implementar a interface IAlunoService
             */
            services.AddScoped<IAlunoService, AlunosService>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AlunosApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AlunosApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
